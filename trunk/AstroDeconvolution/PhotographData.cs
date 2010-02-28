using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace AstroDeconvolution
{
    /**
     * @brief
     * Wrapper around photograph data.
     * 
     * This includes the picture and the list of associated PSF
     */
    [Serializable]
    class PhotographData
    {
        /**
         * @brief
         * Path to the image being analyzed
         */
        public string Image
        {
            get;
            set;
        }

        /**
         * @brief
         * List of the PSFs in the image
         */
        public List<PsfWrapper> PSFs
        {
            get;
            set;
        }

        /**
         * @brief
         * Number of PSF associated with the image
         */
        public int NPsf
        {
            get
            {
                return PSFs.Count;
            }
        }

        /**
         * @brief
         * Default Constructor
         */
        public PhotographData()
        {
            PSFs = new List<PsfWrapper>();
        }

        /**
         * @brief
         * Constructor
         * 
         * Creates Photograph data for the specified image
         * 
         * @param aImage Path to the image
         */
        public PhotographData(string aImage)
        {
            Image = aImage;
            PSFs = new List<PsfWrapper>();
        }

        /**
         * @brief
         * Add a PSF to the data
         * 
         * @param aPsf The PSF to add
         */
        public void AddPsf(PsfWrapper aPsf)
        {
            PSFs.Add(aPsf);
        }

        /**
         * @brief
         * Save the data to a file
         * 
         * @param aFileName Path of the file to save to
         */
        public void Save(string aFileName)
        {
            using (FileStream fileStream = new FileStream(aFileName, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, this);
            }
        }

        /**
         * @brief
         * Load the data from a file
         * 
         * @param aFileName The file to load from
         */
        public static PhotographData  Load(string aFileName)
        {
            using (FileStream fileStream = new FileStream(aFileName, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return (PhotographData) binaryFormatter.Deserialize(fileStream);
            }
        }

        /**
         * @brief
         * Data about a single pixel
         * 
         * This is the list of values of a stack of images
         */
        public struct PixelData
        {
            /**
             * @brief
             * The X position of the pixel
             */
            public int X;

            /**
             * @brief
             * The Y position of the pixel
             */
            public int Y;

            /**
             * @brief
             * The list of values in the stack
             */
            public double[] Value;

            /**
             * @brief
             * Constructor
             * @param x x-position
             * @param y y-position
             * @param value Value of the pixels at the (x, y) position
             */
            public PixelData(int x, int y, double[] value)
            {
                X = x;
                Y = y;
                Value = value;
            }
        }

        /**
         * @brief
         * Enumerator to generate the stack of values for
         * all pixels in the PSF.
         * 
         * The enumerator will return PixelData
         * 
         * For the X, Y pairs that is not defined in a PSF the value will be 0
         */
        public IEnumerable<PixelData> AllPixels
        {
            get
            {
                // Find the largest bounding box
                int Left = 0;
                int Right = 0;
                int Top = 0;
                int Bottom = 0;

                foreach (var psfWrapper in PSFs)
                {
                    PSF psf = psfWrapper.Psf;
                    if (psf.Xmin < Left)
                    {
                        Left = psf.Xmin;
                    }
                    if (psf.Xmax > Right)
                    {
                        Right = psf.Xmax;
                    }
                    if (psf.Ymax > Bottom)
                    {
                        Bottom = psf.Ymax;
                    }
                    if (psf.Ymin < Top)
                    {
                        Top = psf.Ymin;
                    }

                }

                for (int x = Left; x <= Right; x++)
                {
                    for (int y = Top; y <= Bottom; y++)
                    {
                        double[] value = new double[PSFs.Count];
                        for (int i = 0; i < PSFs.Count; i++) {
                            value[i] = PSFs[i].Psf[x, y];
                        }
                        yield return new PixelData(x, y, value);
                    }
                }
            }
        }
    }
}
