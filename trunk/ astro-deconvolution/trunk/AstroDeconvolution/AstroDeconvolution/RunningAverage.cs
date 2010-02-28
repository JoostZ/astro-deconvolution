using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace AstroDeconvolution
{
    class RunningAverage
    {
        ShiftingArray<double> data;

        int Span {
            get;
            set;
        }

        double _average = 0.0;

        public double Average {
            get {
                return _average;
            }
        }

        public RunningAverage(int span)
        {
            data = new ShiftingArray<double>(span);
            Span = span;
        }

        public double Add(double x)
        {
            int size = data.Span;
            if (size < Span)
            {
                _average = (size * _average + x) / (size + 1);
            }
            else
            {
                _average += (x - data[0]) / Span;
            }
            data.Add(x);

            return _average;
        }


    }

#if DEBUG
    [TestFixture]
    public class RunningAverageTest
    {
        readonly int span = 5;
        [Test]
        public void Creation()
        {
            var average = new RunningAverage(span);

            Assert.That(average.Average, Is.EqualTo(0.0));
        }

        [Test]
        public void InitialAverage()
        {
            var average = new RunningAverage(span);

            double[] data = new[] {
                1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0
            };

            for (int i = 0; i < span; i++)
            {
                double avg = average.Add(data[i]);
                Assert.That(average.Average, Is.EqualTo(avg));

                double calculatedAverage = 0.0;
                for (int j = 0; j <= i; j++)
                {
                    calculatedAverage += data[j];
                }
                calculatedAverage /= (i + 1);
                Assert.That(avg, Is.EqualTo(calculatedAverage).Within(0.00001), "for i = "+ i);
            }

            for (int i = span; i < data.Length; i++) {
                double avg = average.Add(data[i]);
                Assert.That(average.Average, Is.EqualTo(avg));

                double calculatedAverage = 0.0;
                for (int j = i - span + 1; j <= i; j++)
                {
                    calculatedAverage += data[j];
                }
                calculatedAverage /= span;
                Assert.That(avg, Is.EqualTo(calculatedAverage).Within(0.00001));
            }
        }
    }
#endif

}
