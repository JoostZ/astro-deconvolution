using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


#if DEBUG
using NUnit.Framework;
#endif

namespace AstroDeconvolution
{
    class RunningCovariance
    {
        private int? Size
        {
            get;
            set;
        }

        public RunningCovariance()
        {
        }

        public double MeanX
        {
            get;
            private set;
        }

        public double MeanY
        {
            get;
            private set;
        }

        public double Covariance
        {
            get
            {
                if (Size.HasValue)
                {
                    return S / (Size.Value - 1);
                }
                else
                {
                    return 0.0;
                }
            }
        }

        private double S
        {
            get;
            set;
        }

        private double Correction
        {
            get;
            set;
        }

        public void Add(double x, double y)
        {
            if (!Size.HasValue)
            {
                MeanX = x;
                MeanY = y;
                S = 0;
                Correction = 0;
                Size = 1;

                return;
            }
            double meanX = MeanX + (x - MeanX) / (Size.Value + 1);
            double meanY = MeanY + (y - MeanY) / (Size.Value + 1);
            if (Size.Value >= 1)
            {
                double covariance =  S + (x - meanX) * (y - MeanY);
                S = covariance;
            }
            MeanX = meanX;
            MeanY = meanY;
            Size++;
        }
    }

#if DEBUG

    [TestFixture]
    public class RunningCovarianceTest
    {
        [Test]
        public void Creation()
        {
            var covariance = new RunningCovariance();

            Assert.That(covariance.MeanX, Is.EqualTo(0.0));
            Assert.That(covariance.MeanY, Is.EqualTo(0.0));
            Assert.That(covariance.Covariance, Is.EqualTo(0.0));
        }

        [Test]
        public void InitialCovariance()
        {
            var covariance = new RunningCovariance();

            int nPoints = 250;

            double[] dataX = new double[nPoints];
            double[] dataY = new double[nPoints];

            for (int i = 0; i < nPoints; i++)
            {
                dataX[i] = i + 1;
                dataY[nPoints - 1 - i] = i + 1;
            }

            for (int i = 0; i < dataX.Length; i++)
            {

                covariance.Add(dataX[i], dataY[i]);

                double meanX = 0;
                double meanY = 0;
                double covar = 0;

                for (int j = 0; j <= i; j++)
                {
                    meanX += dataX[j];
                    meanY += dataY[j];
                }

                meanX /= (i + 1);
                meanY /= (i + 1);
                Assert.That(covariance.MeanX, Is.EqualTo(meanX).Within(0.00001), "Initial meanX i = " + i);
                Assert.That(covariance.MeanY, Is.EqualTo(meanY).Within(0.00001), "Initial meanY i = " + i);
                if (i > 1)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        double termX = dataX[j] - meanX;
                        double termY = dataY[j] - meanY;
                        covar += (termX * termY);
                    }
                    covar /= i;
                    Assert.That(covariance.Covariance, Is.EqualTo(covar).Within(0.0000000001), "Initial variance i = " + i);
                }
            }


        }
    }
#endif

}
