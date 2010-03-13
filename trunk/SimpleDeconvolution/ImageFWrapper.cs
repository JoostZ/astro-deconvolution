using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AstroDeconvolution;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace SimpleDeconvolution
{
    /**
     * @brief
     * Wrapper around ImageF to allow access from WPF
     */
    public class ImageFWrapper
    {
        public ImageFWrapper(ImageF image)
        {
            Image = image;
        }

        internal ImageF Image
        {
            get;
            set;
        }

        private BitmapSource bitmap;
        public BitmapSource Bitmap
        {
            get
            {
                if (bitmap == null)
                {
                    Bitmap = ToBitmap();
                }
                return bitmap;
            }
            set
            {
                bitmap = value;
            }
        }

        private BitmapSource ToBitmap()
        {
            System.Drawing.Color[,] color = Image.ToRawImage();

            int width = color.GetLength(0);
            int height = color.GetLength(1);
            PixelFormat pf = PixelFormats.Rgb24;
            int rawStride = width * ((pf.BitsPerPixel + 7) / 8);
            byte[] rawImage = new byte[rawStride * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    System.Drawing.Color c = color[x, y];
                    rawImage[x * 3 + y * rawStride] = c.R;
                    rawImage[x * 3 + y * rawStride + 1] = c.G;
                    rawImage[x * 3 + y * rawStride + 2] = c.B;
                }
            }

            BitmapSource result = BitmapSource.Create(width, height, 96, 96, pf, null, rawImage, rawStride);
            return result;
        }
    }
}
