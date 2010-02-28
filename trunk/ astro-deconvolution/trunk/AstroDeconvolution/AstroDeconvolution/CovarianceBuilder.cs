using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;



namespace AstroDeconvolution
{
    class CovarianceBuilder
    {
        RunningCovariance[,] iMatrix;

        private PhotographData Data {
            get;
            set;
        }

        public CovarianceBuilder(PhotographData data)
        {
            int size = data.NPsf;
            iMatrix = new RunningCovariance[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = i; j < size; j++) {
                    iMatrix[i, j] = new RunningCovariance();
                }
            }

            Data = data;
        }

        public Matrix Create()
        {
            foreach (var pixel in Data.AllPixels)
            {
                for (int i = 0; i < Data.NPsf; i++)
                {
                    double valueI = pixel.Value[i];
                    for (int j = i; j < Data.NPsf; j++)
                    {
                        double valueJ = pixel.Value[j];
                        iMatrix[i, j].Add(valueI, valueJ);
                    }
                }
            }

            Matrix result = new Matrix(Data.NPsf, Data.NPsf);
            for (int i = 0; i < Data.NPsf; i++)
            {
                for (int j = i; j < Data.NPsf; j++)
                {
                    result[i, j] = iMatrix[i, j].Covariance;
                    if (j != i)
                    {
                        result[j, i] = result[i, j];
                    }
                }
            }
            return result;
        }

    }
}
