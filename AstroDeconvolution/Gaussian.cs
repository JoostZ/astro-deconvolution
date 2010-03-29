using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace AstroDeconvolution
{
    /**
     * @brief
     * Assymetric 2D Gaussian
     */
    public class Gaussian
    {
        /**
         * @brief
         * Constructor
         * 
         * @param x0
         * Origin of Gaussian in X direction
         * 
         * @param y0
         * Origin of Gaussian in Y direction
         * 
         * @param sigmaX
         * Standard deviation in X direction
         * 
         * @param sigmaY
         * Standard deviation in Y direction
         * 
         * @param theta
         * Angle (in degrees) that the distribution is rotated around the origin
         * 
         * @param scale
         * Multiplication factor of the value
         */
        public Gaussian(double x0, double y0, double sigmaX, double sigmaY, double theta, double scale)
        {
            X0 = x0;
            Y0 = y0;
            SigmaX = sigmaX;
            SigmaY = sigmaY;
            Theta = theta;
            CosTheta = Math.Cos(_theta * ToRad);
            SinTheta = Math.Sin(_theta * ToRad);
            Scale = scale;
        }

        /**
         * @brief
         * Origin of Gaussian in X direction
         */
        public double X0
        {
            get;
            set;
        }

        /**
         * @brief
         * Origin of Gaussian in Y direction
         */
        public double Y0
        {
            get;
            set;
        }

        private double _theta;
        /**
         * @brief 
         * Angle (in degrees) that the distribution is rotated around the origin
         */
        public double Theta
        {
            get {
                return _theta;
            }
            set
            {
                if (_theta != value)
                {
                    _theta = value;
                    CosTheta = Math.Cos(_theta * ToRad);
                    SinTheta = Math.Sin(_theta * ToRad);
                }
            }
        }

        private double _sigmaX;
        /**
         * @brief
         * Standard deviation in X direction
         */
        public double SigmaX
        {
            get {
                return _sigmaX;
            }
            set
            {
                if (_sigmaX != value)
                {
                    _sigmaX = value;
                    SigmaX2 = value * value;
                }
            }
        }

        private double _sigmaY;
        /**
         * @brief
         * Standard deviation in X direction
         */
        public double SigmaY
        {
            get
            {
                return _sigmaY;
            }
            set
            {
                if (_sigmaY != value)
                {
                    _sigmaY = value;
                    SigmaY2 = value * value;
                }
            }
        }

        /**
         * @brief
         * Multiplication factor of the value
         */
        public double Scale {
            get; set; 
        }

        private double SigmaX2
        {
            get;
            set;
        }
        private double SigmaY2
        {
            get;
            set;
        }
        private const double ToRad = Math.PI / 180.0;
        private const double ToDeg = 180.0 / Math.PI;

        private double CosTheta
        {
            get;
            set;
        }

        private double SinTheta
        {
            get;
            set;
        }

        /**
         * @brief
         * Value of the Gaussian at (x, y)
         * 
         * @param x
         * X-coordinate where the value is calculated
         * 
         * @param y
         * Y-coordinate where the value is calculated
         */
        public double this[double x, double y]
        {
            get
            {

                double dX = x - X0;
                double dY = y - Y0;

                double xPrime = dX * CosTheta - dY * SinTheta;
                double yPrime = dX * SinTheta + dY * CosTheta;

                return Scale * Math.Exp(-0.5 * (xPrime * xPrime / SigmaX2 + yPrime * yPrime / SigmaY2));
            }
        }

        /**
         * @brief
         * The parameters of the Gaussian
         */
        public enum Parameters
        {
            Scale,
            X0,
            Y0,
            SigmaX,
            SigmaY,
            Theta
        };

        /**
         * @brief
         * Value of all the parameters
         */
        public double[] ParameterValues
        {
            get
            {
                double[] result = new double[6];
                result[(int)Parameters.Scale] = Scale;
                result[(int)Parameters.X0] = X0;
                result[(int)Parameters.Y0] = Y0;
                result[(int)Parameters.SigmaX] = SigmaX;
                result[(int)Parameters.SigmaY] = SigmaY;
                result[(int)Parameters.Theta] = Theta;
                return result;
            }
        }
        #region partial derivatives

        /**
         * @brief
         * Value of all partial derivatives at point (x, y)
         * 
         * @param x
         * X-value at which to evaluate the partial derivatives
         * @param y
         * Y-value at which to evaluate the partial derivatives
         */
        public double[] PartialDerivatives(double x, double y)
        {
                double[] result = new double[6];
                result[(int)Parameters.Scale] = PartialDerivativeScale(x, y);
                result[(int)Parameters.X0] = PartialDerivativeX0(x, y);
                result[(int)Parameters.Y0] = PartialDerivativeY0(x, y);
                result[(int)Parameters.SigmaX] = PartialDerivativeSigmaX(x, y);
                result[(int)Parameters.SigmaY] = PartialDerivativeSigmaY(x, y);
                result[(int)Parameters.Theta] = PartialDerivativeTheta(x, y);
                return result;
        }

        /**
         * @brief
         * Partial derivative with respect to Scale
         * 
         * @param x
         * X-value at which to evaluate the partial derivatives
         * @param y
         * Y-value at which to evaluate the partial derivatives
         */
        public double PartialDerivativeScale(double x, double y)
        {
            return this[x, y] / Scale;
        }

        /**
         * @brief
         * Partial derivative with respect to X0
         * 
         * @param x
         * X-value at which to evaluate the partial derivatives
         * @param y
         * Y-value at which to evaluate the partial derivatives
         */
        public double PartialDerivativeX0(double x, double y)
        {
            // F = xPrime / SigmaX + yPrime / SigmaY
            // dF / dX0 = (d xPrime / d X0) / SigmaX
            // xPrime = (cos theta (x - x0) - sin theta * (y - y0)
            // d xPrime / dx0 = - cos theta
                return  this[x, y] * (CosTheta / SigmaX2 + SinTheta / SigmaY2);
        }

        /**
         * @brief
         * Partial derivative with respect to Y0
         * 
         * @param x
         * X-value at which to evaluate the partial derivatives
         * @param y
         * Y-value at which to evaluate the partial derivatives
         */
        public double PartialDerivativeY0(double x, double y)
        {
            // F = xPrime / SigmaX + yPrime / SigmaY
            // dF / dX0 = (d yPrime / d Y0) / SigmaY
            // yPrime = (cos theta (x - x0) - sin theta * (y - y0)
            // d yPrime / dy0 =  sin theta
            return this[x, y] * (SinTheta / SigmaX2 - CosTheta / SigmaY2);
        }

        /**
         * @brief
         * Partial derivative with respect to SigmaX
         * 
         * @param x
         * X-value at which to evaluate the partial derivatives
         * @param y
         * Y-value at which to evaluate the partial derivatives
         */
        public double PartialDerivativeSigmaX(double x, double y)
        {
            return 0;
        }

        /**
         * @brief
         * Partial derivative with respect to SigmaY
         * 
         * @param x
         * X-value at which to evaluate the partial derivatives
         * @param y
         * Y-value at which to evaluate the partial derivatives
         */
        public double PartialDerivativeSigmaY(double x, double y)
        {
            return 0;
        }

        /**
         * @brief
         * Partial derivative with respect to Theta
         * 
         * @param x
         * X-value at which to evaluate the partial derivatives
         * @param y
         * Y-value at which to evaluate the partial derivatives
         */
        public double PartialDerivativeTheta(double x, double y)
        {
            return 0;
        }

        #endregion

        public struct BoundingBox
        {
            public double XMin;
            public double XMax;
            public double YMin;
            public double YMax;
        }

        public BoundingBox GetBound(double value)
        {
            BoundingBox result = new BoundingBox();

            double scale = Math.Log(Scale / value);
            // Find a_x
            double a_x = SigmaX * Math.Sqrt(2 * scale);
            double a_y = SigmaY * Math.Sqrt(2 * scale);

            double t = Math.Atan2(-a_y * SinTheta, a_x * CosTheta);
            double cosT = Math.Cos(t);
            double sinT = Math.Sin(t);
            double x1 = X0 + a_x * cosT * CosTheta - a_y * sinT * SinTheta;
            double x2 = X0 - a_x * cosT * CosTheta + a_y * sinT * SinTheta;
            if (x1 > x2)
            {
                result.XMax = x1;
                result.XMin = x2;
            }
            else
            {
                result.XMax = x2;
                result.XMin = x1;
            }

            t = Math.Atan2(a_y * CosTheta, a_x * SinTheta);
            cosT = Math.Cos(t);
            sinT = Math.Sin(t);
            double y1 = Y0 + a_x * cosT * SinTheta + a_y * sinT * CosTheta;
            double y2 = Y0 - a_x * cosT * SinTheta - a_y * sinT * CosTheta;
            if (y1 > y2)
            {
                result.YMax = y1;
                result.YMin = y2;
            }
            else
            {
                result.YMax = y2;
                result.YMin = y1;
            }

            return result;
        }
    }
#if DEBUG
    [TestFixture]
    public class GaussianTest
    {
        [Test]
        public void TestProperties()
        {
            double x0 = 0.0;
            double y0 = 0.0;
            double sigmaX = 1.0;
            double sigmaY = 1.0;
            double theta = 0.0;
            double scale = 1.0;

            Gaussian theGaussian = new Gaussian(x0, y0, sigmaX, sigmaY, theta, scale);
            Assert.That(theGaussian.X0, Is.EqualTo(x0).Within(0.00001), "Initial x0");
            Assert.That(theGaussian.Y0, Is.EqualTo(y0).Within(0.00001), "Initial y0");
            Assert.That(theGaussian.SigmaX, Is.EqualTo(sigmaX).Within(0.00001), "Initial sigma X");
            Assert.That(theGaussian.SigmaY, Is.EqualTo(sigmaY).Within(0.00001), "Initial sigma Y");
            Assert.That(theGaussian.Theta, Is.EqualTo(theta).Within(0.00001), "Initial theta");
            Assert.That(theGaussian.Scale, Is.EqualTo(scale).Within(0.00001), "Initial scale");

            x0 += 10;
            theGaussian.X0 = x0;
            Assert.That(theGaussian.X0, Is.EqualTo(x0).Within(0.00001), "Changed x0");

            y0 += 10;
            theGaussian.Y0 = y0;
            Assert.That(theGaussian.Y0, Is.EqualTo(y0).Within(0.00001), "Changed y0");

            sigmaX += 0.5;
            theGaussian.SigmaX = sigmaX;
            Assert.That(theGaussian.SigmaX, Is.EqualTo(sigmaX).Within(0.00001), "Changed sigma X");

            sigmaY += 0.5;
            theGaussian.SigmaY = sigmaY;
            Assert.That(theGaussian.SigmaY, Is.EqualTo(sigmaY).Within(0.00001), "Changed sigma Y");

            theta += 30;
            theGaussian.Theta = theta;
            Assert.That(theGaussian.Theta, Is.EqualTo(theta).Within(0.00001), "Changed theta");

            scale += 0.5;
            theGaussian.Scale = scale;
            Assert.That(theGaussian.Scale, Is.EqualTo(scale).Within(0.00001), "Changed scale");

            double[] parameters = theGaussian.ParameterValues;

            Assert.That(theGaussian.X0, Is.EqualTo(parameters[(int)Gaussian.Parameters.X0]).Within(0.00001), "Parameter X0");
            Assert.That(theGaussian.Y0, Is.EqualTo(parameters[(int)Gaussian.Parameters.Y0]).Within(0.00001), "Parameter Y0");
            Assert.That(theGaussian.SigmaX, Is.EqualTo(parameters[(int)Gaussian.Parameters.SigmaX]).Within(0.00001), "Parameter SigmaX");
            Assert.That(theGaussian.SigmaY, Is.EqualTo(parameters[(int)Gaussian.Parameters.SigmaY]).Within(0.00001), "Parameter SigmaY");
            Assert.That(theGaussian.Theta, Is.EqualTo(parameters[(int)Gaussian.Parameters.Theta]).Within(0.00001), "Parameter Theta");
            Assert.That(theGaussian.Scale, Is.EqualTo(parameters[(int)Gaussian.Parameters.Scale]).Within(0.00001), "Parameter Scale");
        }


        [Test]
        public void TestValues()
        {
            double x0 = 0.0;
            double y0 = 0.0;
            double sigmaX = 1.0;
            double sigmaY = 1.0;
            double theta = 0.0;
            double scale = 1.0;

            Gaussian theGaussian = new Gaussian(x0, y0, sigmaX, sigmaY, theta, scale);

            for (double x = x0; x < 5 * sigmaX; x += 0.2)
            {
                double expectedValue = scale * Math.Exp(-0.5 * (x - x0) * (x - x0) / (sigmaX * sigmaX));
                Assert.That(theGaussian[x, 0], Is.EqualTo(expectedValue).Within(0.00001), "Value [" + x + ", 0]");
            }
            for (double y = y0; y < 5 * sigmaY; y += 0.2)
            {
                double expectedValue = scale * Math.Exp(-0.5 * (y - y0) * (y - y0) / (sigmaY * sigmaY));
                Assert.That(theGaussian[0, y], Is.EqualTo(expectedValue).Within(0.00001), "Value [" + 0 + ", y]");
            }
        }
    }
#endif
}
