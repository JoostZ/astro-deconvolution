using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FftwConvolver;

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

        PSF(double[,] data, Point origin)
        {
            iData = new List<List<double>>();
            for (int y = 0; y < data.GetLength(1); y++)
            {
                List<double> row = new List<double>();
                for (int x = 0; x < data.GetLength(0); x++)
                {
                    row.Add(data[x, y]);
                }
                iData.Add(row);
            }
            _xmin = -origin.X;
            _xmax = data.GetLength(0) + Xmin - 1;
            _ymin = -origin.Y;
            _ymax = data.GetLength(1) + Ymin - 1;
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

        public static PSF SymmetricGaussian(double sigma)
        {
            double sigma2 = sigma * sigma;

            Point origin = new Point();
            origin.X = (int)(3 * sigma) + 1;
            origin.Y = origin.X;
            int width = 2 * origin.X + 1;
            int height = width;
            double[,] psf = new double[width, height];


            double integral = 0.0;
            for (int x = 0; x < width; x++)
            {
                int x1 = x - origin.X;
                for (int y = 0; y < height; y++)
                {
                    int y1 = y - origin.Y;
                    double radius = (double)(x1 * x1 + y1 * y1);
                    double value = Math.Exp(-0.5 * radius / sigma2);
                    integral += value;
                    psf[x, y] = value;
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    psf[x, y] /= integral;
                }
            }

            return new PSF(psf, origin);
        }

        #region OutConversion
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

        public KernelData ToKernelData()
        {
            var data = new double[Xmin + Xmax + 1, Ymin + Ymax + 1];
            for (int y = Ymin; y <= Ymax; y++)
            {
                List<double> row = iData[y - Ymin];
                for (int x = Xmin; x <= Xmax; x++)
                {
                    data[x - Xmin, y - Ymin] = row[x - Xmin];
                }
            }

            return new KernelData(data, new Point(-Xmin, -Ymin));
 
        }

        #endregion

        private Size FftSize
        {
            get;
            set;
        }


        FftwConvolver.FftwConvolver Convolver
        {
            get;
            set;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder("PSF:\n");
            for (int y = Ymin; y <= Ymax; y++)
            {
                for (int x = Xmin; x <= Xmax; x++)
                {
                    builder.AppendFormat("{0:F4}  ", this[x, y]);
                }
                builder.Append("\n");
            }

            builder.Append("\n");
            return builder.ToString();
        }

        #region IConvolutable Members

        public ImageF Convolute(ImageF image)
        {
            CheckSize(image);

            return ImageF.FromArray(Convolver.Convolve(image.ToRawData));
        }

        private void CheckSize(ImageF image)
        {
            if (Convolver == null || FftSize.Width != image.Width || FftSize.Height != image.Height)
            {
                FftSize = new Size(image.Width, image.Height);
                double[,] imgData = new double[FftSize.Width, FftSize.Height];
                for (int x = 0; x < FftSize.Width; x++)
                {
                    for (int y = 0; y < FftSize.Height; y++)
                    {
                        imgData[x, y] = this[x + Xmin, y + Ymin];
                    }
                }
                Convolver = new FftwConvolver.FftwConvolver(FftSize, imgData, new Point(-Xmin, -Ymin));
            }
        }


        public ImageF ConvoluteTranspose(ImageF image)
        {
            CheckSize(image);
            return ImageF.FromArray(Convolver.Convolve(image.ToRawData));
        }

        #endregion
    }
}
