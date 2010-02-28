using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AstroDeconvolution
{
    /// <summary>
    /// Analyze a small piece of an image along a scan line (horizontal)
    /// </summary>
    class ScanLineAnalyzer
    {

        private ImageF Image
        {
            get;
            set;
        }

        /// <summary>
        /// The scanline being analyzed
        /// </summary>
        public int? ScanLine
        {
            get;
            set;
        }

        /// <summary>
        /// The X-coordinate along the scanline where the analysis starts
        /// </summary>
        public int? XCenter
        {
            get;
            set;
        }

        public int LeftBackgroundPosition
        {
            get;
            set;
        }
        public double LeftBackground
        {
            get;
            set;
        }
        public double LeftBackgroundDeviation
        {
            get;
            set;
        }
        public int RightBackgroundPosition
        {
            get;
            set;
        }
        public double RightBackground
        {
            get;
            set;
        }
        public double RightBackgroundDeviation
        {
            get;
            set;
        }

        public bool IsBackgroundLine
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="image"></param>
        /// <param name="scanLine"></param>
        /// <param name="xCenter"></param>
        public ScanLineAnalyzer(ImageF image, int scanLine, int xCenter)
        {
            Image = image;
            ScanLine = scanLine;
            XCenter = xCenter;
        }

        public ScanLineAnalyzer(ImageF image, int scanLine)
        {
            Image = image;
            ScanLine = scanLine;
        }

        public ScanLineAnalyzer(ImageF image)
        {
            Image = image;
        }

        public void Analyze(int xCenter, int scanLine)
        {
            XCenter = xCenter;
            ScanLine = scanLine;
            Analyze();
        }

        public void Analyze(int xCenter)
        {
            XCenter = xCenter;
            Analyze();
        }

        public bool FindBackground()
        {
            if (!XCenter.HasValue && !ScanLine.HasValue)
            {
                throw new Exception();
            }

            int x = (int)XCenter;
            int y = (int)ScanLine;
            double start = Image[x, y];

            var stats = new RunningStatistics(5);

            stats.Add(Image[x, y]);
            stats.Add(Image[x - 1, y]);

            int dx = x- 1;
            double stdDev;
            double mean;
            double lastVariance;
            do
            {
                lastVariance = stats.Variance;
                stats.Add(Image[--dx, y]);
                stdDev = Math.Sqrt(stats.Variance);
                mean = stats.Mean;
            } while (stats.Variance < lastVariance || Math.Abs(mean - start) < 3 * stdDev);

            LeftBackgroundPosition = dx + 1;
            LeftBackground = mean;
            LeftBackgroundDeviation = stdDev;

            stats.Clear(5);
            stats.Add(Image[x, y]);
            stats.Add(Image[x - 1, y]);

            dx = x + 1;
            do
            {
                lastVariance = stats.Variance;
                stats.Add(Image[++dx, y]);
                stdDev = Math.Sqrt(stats.Variance);
                mean = stats.Mean;
            } while (stats.Variance < lastVariance || Math.Abs(mean - start) < 3 * stdDev);

            RightBackgroundPosition = dx - 1;
            RightBackground = mean;
            RightBackgroundDeviation = stdDev;

            IsBackgroundLine = false;

            return true;
        }

        /// <summary>
        /// Analyze the scanline Scanline around point XCenter
        /// </summary>
        public void Analyze()
        {
            if (!XCenter.HasValue && !ScanLine.HasValue)
            {
                throw new Exception();
            }

            int x = (int)XCenter;
            int y = (int)ScanLine;
            double start = Image[x, y];
            double meanMax = 0.0;

            var stats = new RunningStatistics(5);

            stats.Add(Image[x, y]);
            stats.Add(Image[x - 1, y]);

            int dx = x - 1;
            double stdDev;
            double mean;
            do
            {
                stats.Add(Image[--dx, y]);
                stdDev = Math.Sqrt(stats.Variance);
                mean = stats.Mean;
                if (mean > meanMax)
                {
                    meanMax = mean;
                }
            } while (Math.Abs(mean - LeftBackground) > 3 * LeftBackgroundDeviation);

            LeftBackgroundPosition = dx + 1;
            LeftBackground = mean;
            LeftBackgroundDeviation = stdDev;

            stats.Clear(5);
            stats.Add(Image[x, y]);
            stats.Add(Image[x - 1, y]);

            dx = x + 1;
            do
            {
                stats.Add(Image[++dx, y]);
                stdDev = Math.Sqrt(stats.Variance);
                mean = stats.Mean;
                if (mean > meanMax)
                {
                    meanMax = mean;
                }
            } while (Math.Abs(mean - RightBackground) > 3 * RightBackgroundDeviation);

            RightBackgroundPosition = dx - 1;
            RightBackground = mean;
            RightBackgroundDeviation = stdDev;

            IsBackgroundLine = (Math.Abs(meanMax - LeftBackground) < 3 * LeftBackgroundDeviation) &&
                               (Math.Abs(meanMax - RightBackground) < 3 * RightBackgroundDeviation);

        }

    }
}
