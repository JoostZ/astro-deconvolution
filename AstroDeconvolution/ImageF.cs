using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;


namespace AstroDeconvolution
{
    /**
     * @brief
     * Monochrome image where the pixels are stored as floating point numbers
     * 
     * The image is read-only and can only be created by static methods or (protected) constructors 
     */
    public class ImageF
    {
        // The image data
        private double[,] imageData;

        /**
         * @brief
         * Get the value of a pixel
         * 
         * @param x X-value of the pixel
         * @param y Y-value of the pixel
         * 
         * @return the value of the pixel at {x,y}
         */
        public double this[int x, int y] 
        {
            get
            {
                return imageData[x, y];
            }
        }

        /**
         * @brief
         * Get the width of the image
         */
        public int Width
        {
            get
            {
                return imageData.GetLength(0);
            }
        }

        /**
         * @brief
         * Get the height of the image
         */
        public int Height
        {
            get
            {
                return imageData.GetLength(1);
            }
        }
                

        /**
         * @brief
         * Default constructor. Creates an empty 0-dimensional image
         */
        private ImageF()
        {
            imageData = null;
        }

        /**
         * @brief
         * Constructor creates an empty image with sepcified width and height
         * 
         * @param width Width of the image
         * @param height Height of the image
         */
        private ImageF(int width, int height)
        {
            imageData = new double[width, height];
        }
        #region creators
        /**
         * @brief
         * Create an ImageF from a two-dimensional array
         * 
         * @param array The two dimensional arry to wrap in an ImageF
         * @return The created ImageF
         */
        static public ImageF FromArray(double[,] array)
        {
            ImageF result = new ImageF();
            result.imageData = array;
            return result;
        }

        static public ImageF FromFile(string filename)
        {
            Bitmap bmp = new Bitmap(filename);
            return FromBitmap(bmp);
        }
        /**
         * @brief
         * Creat an ImageF from a Bitmap
         * 
         * The function takes the intensity of each pixel by adding the RGB values
         * 
         * @param bm The bitmap to wrap
         * @return The created ImageF
         * 
         * @note The funcion is declared <i>unsafe</i> since it uses pointers to the
         * bitmap data to speed-up the process ofg conversion
         */
        unsafe static public ImageF FromBitmap(Bitmap bm)
        {
            Rectangle rect = new Rectangle(new Point(0, 0), bm.Size);
            BitmapData pixels = bm.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            var image = new ImageF(bm.Width, bm.Height);

            for (int y = 0; y < bm.Height; y++)
            {
                int scan = y * pixels.Stride;
                for (int x = 0; x < bm.Width; x++)
                {
                    int offset = scan + 3 * x;
                    byte r = ((byte*)pixels.Scan0)[offset];
                    byte g = ((byte*)pixels.Scan0)[offset + 1];
                    byte b = ((byte*)pixels.Scan0)[offset + 2];

                    double v = (r + g + b) / (256.0 * 3.0);
                    image.imageData[x, y] = v;
                }
            }
            bm.UnlockBits(pixels);

            return image;
        }

        /**
         * @brief
         * Create an image with a constant pixel value
         * 
         * @param width Width of the image
         * @param height Height of the omage
         * @param value Value to set each pixel to
         */
        public static ImageF ConstantImage(int width, int height, double value)
        {
            ImageF result = new ImageF(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    result.imageData[i, j] = value;
                }
            }
            return result;
        }

        public static ImageF RandomPixelImage(int width, int height, int nPoints)
        {
            ImageF result = new ImageF(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    result.imageData[x, y] = 0.0;
                }
            }

            result.imageData[width / 2, height / 2] = 1.0;
            //Random rand = new Random();
            //for (int i = 0; i < nPoints; i++)
            //{
            //    int x = rand.Next(width);
            //    int y = rand.Next(height);

            //    result.imageData[x, y] = 1.0;
            //}
            return result;
        }
        #endregion

        #region Arithmetic Operators
        /**
         * @brief
         * Add an ImageF pixelwise to this object
         * 
         * @param rhs
         * ImageF object to add
         * 
         * @return
         * The result of the addition
         */
        public ImageF Add(ImageF rhs)
        {
            Debug.Assert(Width == rhs.Width && Height == rhs.Height);
            var result = new ImageF(Width, Height);
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    result.imageData[i, j] = this[i, j] + rhs[i, j];
                }
            }
            return result;
        }

        /**
         * @brief
         * Add two ImageF objects pixel by pixel
         * 
         * @param lhs First of the ImageF objects
         * @param rhs Second of the ImageF objects
         * 
         * @return
         * The result of the addition
         */
        public static ImageF operator +(ImageF lhs, ImageF rhs)
        {
            return lhs.Add(rhs);
        }

        /**
         * @brief
         * Divide this object by an ImageF pixelwise
         * 
         * @param rhs
         * ImageF object to divide by
         * 
         * @return
         * The result of the division
         * 
         * @note Even ifg a pixel in rhs is 0, the division will not fail
         */
        public ImageF Divide(ImageF rhs)
        {
            double  Limit = 0.0000001;
            ImageF result = new ImageF(Width, Height);
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++) {
                    result.imageData[i, j] = this[i, j] / Math.Max(rhs[i, j], Limit);
                }
            }
            return result;
        }
        /**
         * @brief
         * Divide two ImageF objects pixel by pixel
         * 
         * @param lhs First of the ImageF objects
         * @param rhs Second of the ImageF objects
         * 
         * @return
         * The result of the division
         */
        public static ImageF operator /(ImageF lhs, ImageF rhs)
        {
            return lhs.Divide(rhs);
        }

        /**
         * @brief
         * Multiply this object with an ImageF pixelwise
         * 
         * @param rhs
         * ImageF object to multiply with
         * 
         * @return
         * The result of the multiplication
         */
        public ImageF Multiply(ImageF rhs)
        {
            ImageF result = new ImageF(Width, Height);
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    result.imageData[i, j] = this[i, j] * rhs[i, j];
                }
            }
            return result;
        }
        /**
         * @brief
         * Multiply two ImageF objects pixel by pixel
         * 
         * @param lhs First of the ImageF objects
         * @param rhs Second of the ImageF objects
         * 
         * @return
         * The result of the multiplication
         */
        public static ImageF operator *(ImageF lhs, ImageF rhs)
        {
            return lhs.Multiply(rhs);
        }
        #endregion

        public Color[,] ToRawImage()
        {
            Color[,] result = new Color[Width, Height];
           
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    double value = this.imageData[x, y];
                    int color = (int)(value * 255);
                    if (color < 0) {
                        color = 0;
                    }
                    if (color > 255) {
                        color = 255;
                    }
                    result[x, y] = Color.FromArgb(color, color, color);
                }
            }

            return result;
        }

        public double[,] ToRawData
        {
            get
            {
                return imageData;
            }
        }
            
        /**
         * @brief
         * Convolute the %PSF into this ImageF
         * 
         * @param psf The %PSF to convolve
         * 
         * @return The result of the convolution
         */
        public ImageF Convolute(PSF psf)
        {
            // TODO: Implement
            return null;
        }
    }
}
