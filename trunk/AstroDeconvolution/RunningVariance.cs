using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace AstroDeconvolution
{
    class RunningStatistics
    {
          ShiftingArray<double> data;

        int Span {
            get;
            set;
        }

        double _mean = 0.0;
        double _variance = 0.0;

        public double Variance {
            get {
                return   _variance;
            }
        }

        public double Mean
        {
            get
            {
                return _mean;
            }
        }

        public RunningStatistics(int span)
        {
            Clear(span);
        }

        public void Clear(int span)
        {
            data = new ShiftingArray<double>(span);
            Span = span;

            _mean = 0;
            _variance = 0;
        }
        public void Add(double x)
        {
            int size = data.Span;
            if (size < Span)
            {

                double mean = Mean + (x - Mean) / (size + 1);
                double variance = 0.0;
                if (size != 0)
                {
                    variance = size * Variance;
                    variance += (x - Mean) * (x - mean);
                    variance /= (size + 1);
                }
                _mean = mean;
                _variance = variance;
            }
            else
            {
                //Var[b] = [ (n-1)*Var[A] + n*Abar^2 + (x6^2 - x1^2) - n*Bbar^2 ] / (n-1)
                double mean = Mean + (x - data[0]) / Span;
                _variance = (Span * Variance + Span * (Mean * Mean - mean * mean) + x * x - data[0]  * data[0]) / Span;
                _mean = mean;

            }
            data.Add(x);

        }


    }

#if DEBUG
    [TestFixture]
    public class RunningStatisticsTest
    {
        readonly int span = 5;

        [Test]
        public void Creation()
        {
            var average = new RunningStatistics(span);

            Assert.That(average.Mean, Is.EqualTo(0.0));
            Assert.That(average.Variance, Is.EqualTo(0.0));
        }

        [Test]
        public void InitialAverage()
        {
            var statistic = new RunningStatistics(span);

            double[] data = new[] {
                1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0
            };

            for (int i = 0; i < span; i++)
            {
                statistic.Add(data[i]);

                double mean = 0;
                double variance = 0;

                for (int j = 0; j <= i; j++)
                {
                    mean += data[j];
                }
                mean /= i + 1;

                Assert.That(statistic.Mean, Is.EqualTo(mean).Within(0.00001), "Initial mean i = " + i);
                if (i != 0)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        double term = data[j] - mean;
                        variance += (term * term);
                    }
                    variance /= (i + 1);
                }
                Assert.That(statistic.Variance, Is.EqualTo(variance).Within(0.00001), "Initial variance i = " + i);
            }

            for (int i = span; i < data.Length; i++)
            {
                statistic.Add(data[i]);

                double mean = 0.0;
                double variance = 0;

                for (int j = i - span + 1; j <= i; j++)
                {
                    mean += data[j];
                }
                mean /= span;
                Assert.That(statistic.Mean, Is.EqualTo(mean).Within(0.00001), "Running mean i = " + i);

                for (int j = i - span + 1; j <= i; j++)
                {
                    double term = data[j] - mean;
                    variance += (term * term);
                }
                variance /= (span);
                Assert.That(statistic.Variance, Is.EqualTo(variance).Within(0.00001), "Running variance i = " + i);
            }
        }
    }
#endif          
}
