using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AstroDeconvolution
{

    using NUnit.Framework;

    class ShiftingArray<T>
    {
        List<T> data = new List<T>();

        public int Span
        {
            get
            {
                return data.Count;
            }
        }

        int Size
        {
            get;
            set;
        }

        public ShiftingArray(int size)
        {
            Size = size;
        }

        public void Add(T value)
        {
            if (data.Count >= Size)
            {
                data.RemoveAt(0);
            }
            data.Add(value);
        }

        public T this[int idx]
        {
            get
            {
                return data[idx];
            }
        }

    }
#if DEBUG
    [TestFixture]
    public class ShiftingArrayTest
    {
        [Test]
        public void Creation()
        {
            var array = new ShiftingArray<double>(5);

            Assert.That(array.Span, Is.EqualTo(0));
        }

        [Test]
        public void FillArray()
        {
            var array = new ShiftingArray<int>(5);
            var list = new int[] {
                1, 2, 3, 4, 5, 6, 7, 8, 9
            };

            for (int i = 0; i < 5; i++)
            {
                array.Add(list[i]);
                Assert.That(array.Span, Is.EqualTo(i + 1));
                Assert.That(array[i], Is.EqualTo(list[i]));
            }

            for (int i = 5; i < list.Length; i++)
            {
                array.Add(list[i]);
                Assert.That(array.Span, Is.EqualTo(5));
                Assert.That(array[0], Is.EqualTo(list[i - 4]));
                Assert.That(array[4], Is.EqualTo(list[i]));
            }
        }


    }
#endif
}
