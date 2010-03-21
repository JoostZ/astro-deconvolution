// FftwConvolver.h

#pragma once
#include "../FFTW/fftw3.h"
#include "KernelData.h"

using namespace System;
using namespace System::Drawing;

namespace FftwConvolver {

	public ref class FftwConvolver
	{
    public:
        /**
         * @brief
         * Constructor
         *
         * @param size
         * Size of the image to be convolved
         *
         * @param psf
         * 2-D array containing the data of the PSF to convolve with
         *
         * @param origin
         * Offset in psf that corresponds to the origin of the PSF
         */
        FftwConvolver(Size size, array<double,2>^ psf, Point origin);

        FftwConvolver(Size size, KernelData^ kernel);
        
        array<double,2>^ Convolve(array<double,2>^ image);
    private:
        void Init(Size size, array<double,2>^ psf, Point origin);

        fftw_complex* TransformPsf(Size size, array<double,2>^ psf, Point origin);
        fftw_complex* TransformPsfTransposed(Size size, array<double,2>^ psf, Point origin);

        fftw_complex* DoConvolve(double* in, fftw_complex* out);
        fftw_complex* DoConvolve(double* in, int bufferSize);
        
        fftw_plan m_planForward;
        fftw_plan m_planBackward;

        double* m_in;
        fftw_complex *m_out;

        fftw_complex *m_psfFft;
        fftw_complex *m_psfTransposeFft;
	};
}
