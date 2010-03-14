using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exocortex.DSP;

namespace AstroDeconvolution
{
    /// <summary>
    /// PixelData Spread Function
    /// </summary>
    [Serializable]
    public class PSF : IConvolutable
    {
        [NonSerialized]Bitmap _bitmap = null;

        public Bitmap Bitmap
        {
            get
            {
                if (_bitmap == null)
                {
                    // Find the maximum
                    double maxValue = 0;
                    for (int y = Ymin; y <= Ymax; y++)
                    {
                        List<double> row = iData[y - Ymin];
                        for (int x = Xmin; x <= Xmax; x++)
                        {
                            if (row[x - Xmin] > maxValue)
                            {
                                maxValue = row[x - Xmin];
                            }
                        }
                    }
                    double scale = 255 / maxValue;
                    _bitmap = new Bitmap(Xmax - Xmin + 1, Ymax - Ymin + 1, PixelFormat.Format24bppRgb);
                    for (int y = Ymin; y <= Ymax; y++)
                    {
                        List<double> row = iData[y - Ymin];
                        for (int x = Xmin; x <= Xmax; x++)
                        {
                            int value = (int)(row[x - Xmin] * scale + 0.5);

                            _bitmap.SetPixel(x - Xmin, y - Ymin, Color.FromArgb(value, value, value));
                        }

                    }
                }
                return _bitmap;
            }
        }
        int _xmin;
        int _xmax;
        int _ymin;
        int _ymax;

        public int Xmin
        {
            get
            {
                return _xmin;
            }
            set
            {
                int diff = _xmin - value;

                if (diff == 0)
                {
                    return;
                }
                else if (diff > 0)
                {
                    PadLeft(diff);
                }
                else
                {
                    StripLeft(-diff);
                }
                _xmin = value;

            }
        }

        public int Xmax
        {
            get
            {
                return _xmax;
            }
            set
            {
                int diff = _xmax - value;

                if (diff == 0)
                {
                    return;
                }
                else if (diff > 0)
                {
                    PadRight(diff);
                }
                else
                {
                    StripRight(-diff);
                }
                _xmax = value;
            }
        }
        public int Ymin
        {
            get
            {
                return _ymin;
            }
            set
            {
                int diff = _ymin - value;

                if (diff == 0)
                {
                    return;
                }
                else if (diff > 0)
                {
                    PadTop(diff);
                }
                else
                {
                    StripTop(-diff);
                }
                _ymin = value;
            }
        }
        public int Ymax
        {
            get
            {
                return _ymax;
            }
            set
            {
                int diff = _ymax - value;

                if (diff == 0)
                {
                    return;
                }
                else if (diff > 0)
                {
                    PadBottom(diff);
                }
                else
                {
                    StripBottom(-diff);
                }
                _ymax = value;
            }
        }

        void PadLeft(int n)
        {
            double[] padd = new double[n];

            foreach (List<double> row in iData)
            {
                row.InsertRange(0, padd);
            }
            _xmin -= n;
        }

        void PadRight(int n)
        {
            double[] padd = new double[n];

            foreach (List<double> row in iData)
            {
                row.AddRange(padd);
            }
            _xmax += n;
        }

        void PadTop(int n)
        {
            List<double> row = new List<double>();
            List<double>[] set = new List<double>[n];
            for (int i = 0; i < n; i++)
            {
                set[i] = row;
            }
            iData.InsertRange(0, set);
            _ymin -= n;
        }

        void PadBottom(int n)
        {
            List<double> row = new List<double>();
            List<double>[] set = new List<double>[n];
            for (int i = 0; i < n; i++)
            {
                set[i] = row;
            }
            iData.AddRange(set);
            _ymax += n;
        }

        void StripLeft(int n)
        {
            if (n >= iData[0].Count)
            {
                throw new Exception();
            }
            foreach (List<double> row in iData)
            {
                row.RemoveRange(0, n);
            }
            _xmin += n;
        }

        void StripRight(int n)
        {
            int start = iData[0].Count - n;
            if (start <= 0)
            {
                throw new Exception();
            }

            foreach (List<double> row in iData)
            {
                row.RemoveRange(start, n);
            }
            _xmax -= n;
        }

        void StripTop(int n)
        {
            if (n >= iData.Count)
            {
                throw new Exception();
            }
            iData.RemoveRange(0, n);
            _ymin += n;
        }

        void StripBottom(int n)
        {
            int start = iData.Count - n;
            if (start <= 0)
            {
                throw new Exception();
            }

            iData.RemoveRange(start, n);
            _ymax -= n;
        }

        List<List<double>> iData;

        public double this[int x, int y]
        {
            get
            {
                if (y < Ymin || y > Ymax)
                {
                    return 0;
                }
                if (x < Xmin || x > Xmax)
                {
                    return 0;
                }

                return iData[y - Ymin][x - Xmin];
            }
            set
            {
                if (y < Ymin || y > Ymax)
                {
                    throw new Exception();
                }
                if (x < Xmin || x > Xmax)
                {
                    throw new Exception();
                }

                int yOffset = y - Ymin;

                List<double> row = iData[y - Ymin];
                row[x - Xmin] = value;
            }
        }

        PSF()
        {
            List<double> row = new List<double>();
            row.Add(1.0);
            iData = new List<List<double>>();
            iData.Add(row);
        }

        public void Normalize()
        {
            double xCenter = 0;
            double yCenter = 0;
            double cog = 0;

            for (int y = Ymin; y <= Ymax; y++)
            {
                List<double> row = iData[y - Ymin];
                for (int x = Xmin; x <= Xmax; x++) {
                    double value = row[x - Xmin];
                    xCenter += x * value;
                    yCenter += y * value;
                    cog += value;
                }
            }
            int size = (Xmax - Xmin + 1) * (Ymax - Ymin + 1);
            xCenter /= cog;
            yCenter /= cog;

            for (int y = Ymin; y <= Ymax; y++)
            {
                List<double> row = iData[y - Ymin];
                for (int x = Xmin; x <= Xmax; x++) {
                    row[x - Xmin] /= cog;
                }
            }

            _xmin -= (int)xCenter;
            _xmax -= (int)xCenter;

            _ymin -= (int)yCenter;
            _ymax -= (int)yCenter;



        }

        unsafe static private double GreyValue(BitmapData aPixels, int x, int y)
        {
            int offset = (y * aPixels.Stride) + (x * 3);

            byte r = ((byte*)aPixels.Scan0)[offset];
            byte g = ((byte*)aPixels.Scan0)[offset + 1];
            byte b = ((byte*)aPixels.Scan0)[offset + 2];

            return (r + g + b) / (256.0 * 3.0);
        }

        unsafe static public PSF FromBitmap(ImageF image, Point pos)
        {
            PSF thePsf = new PSF();
            

            ScanLineAnalyzer analyzer = new ScanLineAnalyzer(image, pos.Y, pos.X);
            analyzer.FindBackground();

            thePsf._xmin = analyzer.LeftBackgroundPosition;
            thePsf._xmax = analyzer.RightBackgroundPosition;

            List<double> row = new List<double>();
            for (int x = analyzer.LeftBackgroundPosition; x <= analyzer.RightBackgroundPosition; x++)
            {
                row.Add(image[x, pos.Y]);
            }
            thePsf.iData[0] = row;
            do {
                analyzer.ScanLine--;
                analyzer.Analyze();
                if (analyzer.LeftBackgroundPosition < thePsf.Xmin)
                {
                    thePsf.Xmin = analyzer.LeftBackgroundPosition;
                } 
                if (analyzer.RightBackgroundPosition > thePsf.Xmax) {
                    thePsf.Xmax = analyzer.RightBackgroundPosition;
                }

                row = new List<double>();
                for (int x = thePsf.Xmin; x <= thePsf.Xmax; x++)
                {
                    if (x < analyzer.LeftBackgroundPosition || x > analyzer.RightBackgroundPosition)
                    {
                        row.Add(0.0);
                    }
                    else {
                        row.Add(image[x, (int)analyzer.ScanLine]);
                    }
                }
                thePsf.iData.Insert(0, row);
                thePsf._ymin = (int)analyzer.ScanLine;
            } while(!analyzer.IsBackgroundLine);

            analyzer.ScanLine = pos.Y;
            do
            {
                analyzer.ScanLine++;
                analyzer.Analyze();
                if (analyzer.LeftBackgroundPosition < thePsf.Xmin)
                {
                    thePsf.Xmin = analyzer.LeftBackgroundPosition;
                }
                if (analyzer.RightBackgroundPosition > thePsf.Xmax)
                {
                    thePsf.Xmax = analyzer.RightBackgroundPosition;
                }

                row = new List<double>();
                for (int x = thePsf.Xmin; x <= thePsf.Xmax; x++)
                {
                    if (x < analyzer.LeftBackgroundPosition || x > analyzer.RightBackgroundPosition)
                    {
                        row.Add(0.0);
                    }
                    else
                    {
                        row.Add(image[x, (int)analyzer.ScanLine]);
                    }
                }
                thePsf.iData.Add(row);
                thePsf._ymax = (int)analyzer.ScanLine;
            } while (!analyzer.IsBackgroundLine);

            thePsf.Normalize();

            return thePsf;
        }

        public Color[,] ToRawImage()
        {
            // Find the maximum
            double maxValue = 0;
            for (int y = Ymin; y <= Ymax; y++)
            {
                List<double> row = iData[y - Ymin];
                for (int x = Xmin; x <= Xmax; x++)
                {
                    if (row[x - Xmin] > maxValue)
                    {
                        maxValue = row[x - Xmin];
                    }
                }
            }
            double scale = 255 / maxValue;

            Color[,] result = new Color[Xmax - Xmin + 1, Ymax - Ymin + 1];

            _bitmap = new Bitmap(Xmax - Xmin + 1, Ymax - Ymin + 1, PixelFormat.Format24bppRgb);
            for (int y = Ymin; y <= Ymax; y++)
            {
                List<double> row = iData[y - Ymin];
                for (int x = Xmin; x <= Xmax; x++)
                {
                    int value = (int)(row[x - Xmin] * scale + 0.5);
                    result[x - Xmin, y - Ymin] = Color.FromArgb(value, value, value);
                }

            }

            return result;
        }

        private Size FftSize
        {
            get;
            set;
        }

        Complex[] FftPsf
        {
            get;
            set;
        }

        Complex[] GetPsfFft(ImageF image)
        {
            double[,] grid = new double[FftSize.Width, FftSize.Height];
            Complex[] data = new Complex[FftSize.Width * FftSize.Height];

            for (int y = Ymin; y <= Ymax; y++)
            {
                int yOffset = y < 0 ? FftSize.Height : 0;
                for (int x = Xmin; x <= Xmax; x++)
                {
                    int xOffset = x < 0 ? FftSize.Width : 0;
                    grid[xOffset + x, yOffset + y] = this[x, y];
                }
            }

            int idx = 0;
            for (int y = 0; y < FftSize.Height; y++)
            {
                for (int x = 0; x < FftSize.Width; x++)
                {
                    data[idx++].Re = (((x + y) & 0x1) != 0) ? -grid[x, y] : grid[x, y];
                }
            }

            Fourier.FFT2(data, FftSize.Width, FftSize.Height, FourierDirection.Forward);


            double scale = 1f / (float)Math.Sqrt(FftSize.Width * FftSize.Height);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] *= scale;
            }
            return data;
        }

        Complex[] FftPsfTranspose
        {
            get;
            set;
        }

        Complex[] GetPsfTransposeFft(ImageF image)
        {
            double[,] grid = new double[FftSize.Width, FftSize.Height];
            Complex[] data = new Complex[FftSize.Width * FftSize.Height];

            for (int y = Ymin; y <= Ymax; y++)
            {
                int yOffset = y < 0 ? FftSize.Width : 0;
                for (int x = Xmin; x <= Xmax; x++)
                {
                    int xOffset = x < 0 ? FftSize.Height : 0;
                    grid[yOffset + y, xOffset + x] = this[x, y];
                }
            }

            int idx = 0;
            for (int y = 0; y < FftSize.Height; y++)
            {
                for (int x = 0; x < FftSize.Width; x++)
                {
                    data[idx++].Re = (((x + y) & 0x1) != 0) ? -grid[x, y] : grid[x, y];
                }
            }

            Fourier.FFT2(data, FftSize.Width, FftSize.Height, FourierDirection.Forward);


            double scale = 1f / (float)Math.Sqrt(FftSize.Width * FftSize.Height);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] *= scale;
            }
            return data;
        }

        Complex[] FftImage(ImageF image)
        {
            Complex[] data = new Complex[FftSize.Width * FftSize.Height];

            for (int y = 0; y < image.Height; y++)
            {
                int rowOffset = y * FftSize.Width;
                for (int x = 0; x < image.Width; x++)
                {
                    data[rowOffset + x].Re = image[x, y];
                    if (((x + 1) & 0x1) != 0)
                    {
                        data[rowOffset + x] *= 1;
                    }
                }
            }

            Fourier.FFT2(data, FftSize.Width, FftSize.Height, FourierDirection.Forward);

            double scale = 1f / (float)Math.Sqrt(FftSize.Width * FftSize.Height);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] *= scale;
            }

            return data;
        }
        #region IConvolutable Members

        public ImageF Convolute(ImageF image)
        {
            Size oldSize = FftSize;
            FftSize = new Size(
                (int)Math.Pow(2, Math.Ceiling(Math.Log(image.Width, 2))),
                (int)Math.Pow(2, Math.Ceiling(Math.Log(image.Height, 2))));

            if (FftPsf == null || FftSize != oldSize)
            {
                FftPsf = GetPsfFft(image);
            }

            return DoConvolute(image, FftPsf);
        }

        private ImageF DoConvolute(ImageF image, Complex[] psf)
        {
            Complex[] imageFft = FftImage(image);

            Complex[] data = new Complex[FftSize.Width * FftSize.Height];

            for (int idx = 0; idx < data.Length; idx++)
            {
                data[idx] = imageFft[idx] * psf[idx];
            }

            Fourier.FFT2(data, FftSize.Width, FftSize.Height, FourierDirection.Backward);

            Double[,] result = new Double[image.Width, image.Height];

            double scale = 1f / (float)Math.Sqrt(FftSize.Width * FftSize.Height);
            int offset = 0;

            for (int y = 0; y < image.Height; y++)
            {
                offset = y * FftSize.Width;
                for (int x = 0; x < image.Width; x++)
                {
                    if (((x + y) & 0x1) != 0)
                    {
                        result[x, y] = -1 * scale * data[offset + x].Re;
                    }
                    else
                    {
                        result[x, y] = scale * data[offset + x].Re;
                    }
                    offset++;
                }
            }

            return ImageF.FromArray(result);
        }


        public ImageF ConvoluteTranspose(ImageF image)
        {
            Size oldSize = FftSize;
            FftSize = new Size(
                (int)Math.Pow(2, Math.Ceiling(Math.Log(image.Width, 2))),
                (int)Math.Pow(2, Math.Ceiling(Math.Log(image.Height, 2))));

            if (FftPsfTranspose == null || FftSize != oldSize)
            {
                FftPsfTranspose = GetPsfTransposeFft(image);
            }

            return DoConvolute(image, FftPsfTranspose);
        }

        #endregion
    }
}
