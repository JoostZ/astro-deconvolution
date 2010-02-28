using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace AstroDeconvolution
{
    class HotellingTransform
    {
        Matrix transformMatrix = null;

        RunningCovariance[,] covarianceMatrix;
        double[] meanVector;

        public int Size
        {
            get;
            set;
        }

        int SampleSize
        {
            get;
            set;
        }

        public double Mean(int idx)
        {
            return covarianceMatrix[idx, idx].MeanX;
        }


        public HotellingTransform(int size)
        {
            Size = size;
            Init(size);
        }
        public HotellingTransform(IEnumerable<double[]> vectors)
        {
            Init(vectors.ElementAt(0).Count());
            foreach (var vector in vectors)
            {
                Add(vector);
            }
        }

        private void Init(int size)
        {
            Size = size;
            covarianceMatrix = new RunningCovariance[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = i; j < Size; j++)
                {
                    covarianceMatrix[i, j] = new RunningCovariance();
                }
            }

            meanVector = new double[Size];
            SampleSize = 0;
        }

        public void Add(double[] X)
        {
            for (int i = 0; i < Size; i++)
            {
                meanVector[i] = (meanVector[i] * SampleSize + X[i]) / (SampleSize + 1);
                for (int j = i; j < Size; j++)
                {
                    covarianceMatrix[i, j].Add(X[i], X[j]);
                }
            }
            SampleSize++;
        }

        internal struct EigenElement
        {
            internal double Value;
            internal double[] Vector;
            internal EigenElement(EigenvalueDecomposition data, int idx)
            {
                Value = data.RealEigenvalues[idx];
                Vector = data.EigenVectors.GetColumnVector(idx).CopyToArray();
            }
        }


        static IEnumerable<EigenElement> EigenList(EigenvalueDecomposition data)
        {
            int size = data.EigenValues.Count();
            for (int i = 0; i < size; i++)
            {
                yield return new EigenElement(data, i);
            }
        }

        private void CalculateTransform()
        {
            Matrix theCovariance = new Matrix(Size, Size);
            Vector means = new Vector(Size);
            // Build the covariance matrix and mean vector
            for (int i = 0; i < Size; i++)
            {
                means[i] = covarianceMatrix[i, i].MeanX;
                for (int j = i; j < Size; j++)
                {
                    theCovariance[i, j] = covarianceMatrix[i, j].Covariance;
                    if (i != j)
                    {
                        theCovariance[j, i] = theCovariance[i, j];
                    }
                }
            }

            List<double[]> theTransform = new List<double[]>();
            // Determine the eigen values and eigen vectors
            EigenvalueDecomposition eigen = theCovariance.EigenvalueDecomposition;
            var r = from e in EigenList(eigen)
                    orderby e.Value descending
                    select e.Vector;
            foreach (var vec in r)
            {
                theTransform.Add(vec);
            }
            transformMatrix = new Matrix(theTransform.ToArray());

        }

        public double[] Transform(double[] X)
        {
            if (transformMatrix == null)
            {
                CalculateTransform();
            }

            Vector mean = new Vector(meanVector);
            Vector xVector = new Vector(X);
            xVector -= mean;
            Vector result = (transformMatrix * xVector.ToColumnMatrix()).GetColumnVector(0);
            return result.CopyToArray();
        }

    }
}
