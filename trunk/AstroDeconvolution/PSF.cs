using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        #region IConvolutable Members

        public ImageF Convolute(ImageF image)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
