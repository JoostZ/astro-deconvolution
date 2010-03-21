// This is the main DLL file.

#include "stdafx.h"
#include <string.h>

#include "FftwConvolver.h"


namespace FftwConvolver {

    FftwConvolver::FftwConvolver(Size size, array<double,2>^ psf, Point origin)
    {
        Init(size, psf, origin);
    }

    FftwConvolver::FftwConvolver(Size size, KernelData^ data)
    {
        Init(size, data->Data, data->Origin);
    }

    void FftwConvolver::Init(Size size, array<double,2>^ psf, Point origin)
    {
        // Allocate buffers for the (real) input image and the complex of FFT
        int bufferSize = size.Width * size.Height;
        m_in = (double *)fftw_malloc(sizeof(double) * bufferSize);
        m_out = (fftw_complex *)fftw_malloc(sizeof(fftw_complex) * bufferSize);

        // Create a plan for the forward FFT ...
        m_planForward = fftw_plan_dft_r2c_2d(size.Width, size.Height, m_in, m_out, FFTW_MEASURE);

        // ... and the backward FFT
        m_planBackward = fftw_plan_dft_c2r_2d(size.Width, size.Height, m_out, m_in, FFTW_MEASURE);


        m_psfFft = TransformPsf(size, psf, origin);
        m_psfTransposeFft = TransformPsfTransposed(size, psf, origin);
    }


    array<double,2>^ FftwConvolver::Convolve(array<double,2>^ image)
    {

        double *p = m_in;
        for (int y = 0; y < image->GetLength(1); y++) {
            for(int x = 0; x < image->GetLength(0); x++)
            {
                *p++ = image[x, y];
            }
        }

        fftw_complex* fft = DoConvolve(m_in, image->Length);

        for (int i = 0; i < image->Length; i++) {
            m_out[i][0] = fft[i][0] * m_psfFft[i][0] - fft[i][1] * m_psfFft[i][1];
            m_out[i][1] = fft[i][0] * m_psfFft[i][1] + fft[i][1] * m_psfFft[i][0];
        }
        
        fftw_execute_dft_c2r(m_planBackward, m_out, m_in);
        
        array<double,2>^ result = gcnew array<double,2>(image->GetLength(0), image->GetLength(1));

        double scale = 1.0 / (image->GetLength(0) * image->GetLength(1));
        p = m_in;
        for (int y = 0; y < image->GetLength(1); y++)
        {
            for (int x = 0; x < image->GetLength(0); x++)
            {
                result[x,y] = *p++ * scale;
            }
        }

        return result;
    }

    fftw_complex* FftwConvolver::TransformPsf(Size size, array<double,2>^ psf, Point origin)
    {
        double* inBuffer = 0;
        fftw_complex* outBuffer = 0;
        int bufferLength = size.Height * size.Width * sizeof(double);
        try {
            array<double,2>^ image = gcnew array<double,2>(size.Width, size.Height);
            inBuffer = (double *)fftw_malloc(bufferLength);
            memset(inBuffer, 0, bufferLength);

            int width = psf->GetLength(0);
            int height = psf->GetLength(1);

            int idx = 0;

            int y;
            for (y = 0; y < height; y++)
            {
                int scanLine = (y - origin.Y);
                int yOffset = 0;
                if (scanLine < 0)
                {
                    scanLine += size.Height;
                }

                yOffset = scanLine * size.Width;
                int x;
                
                int xOffset = yOffset + size.Width - origin.X;
                for (x = 0; x < origin.X; x++) {
                    idx = xOffset + x;
                    inBuffer[idx] = psf[x, y];
                }

                for (; x <width; x++) {
                    idx = yOffset + x - origin.X;
                    inBuffer[idx] = psf[x, y];
                }
            }
            outBuffer = DoConvolve(inBuffer, bufferLength);
        }
        catch (Exception^)
        {
            fftw_free(inBuffer);
            fftw_free(outBuffer);
            throw;
        }
        fftw_free(inBuffer);
        return outBuffer;
    }

    fftw_complex* FftwConvolver::TransformPsfTransposed(Size size, array<double,2>^ psf, Point origin)
    {
        Point theOrigin;
        theOrigin.X = origin.Y;
        theOrigin.Y = origin.X;
        array<double, 2>^ thePsf = gcnew array<double, 2>(theOrigin.X, theOrigin.Y);

        for (int x = 0; x < theOrigin.X; x++)
        {
            for (int y = 0; y < theOrigin.Y; y++)
            {
                thePsf[x, y] = psf[y, x];
            }
        }
        return TransformPsf(size, thePsf, theOrigin);
    }

    fftw_complex* FftwConvolver::DoConvolve(double* in, int bufferSize)
    {
        fftw_complex* buffer = (fftw_complex *)fftw_malloc(sizeof(fftw_complex) * bufferSize);
        return DoConvolve(in, buffer);
    }

    fftw_complex* FftwConvolver::DoConvolve(double* in, fftw_complex* out)
    {
        
        fftw_execute_dft_r2c(m_planForward, in, out);
        return out;
    }
}
