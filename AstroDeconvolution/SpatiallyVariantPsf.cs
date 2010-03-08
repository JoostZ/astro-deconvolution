using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AstroDeconvolution
{
    /**
     * @brief
     * Spatially-variant Point Spread Functions
     * 
     * Besides the actual PSF it contains also the position in an image
     * where this PSF is valid.
     */
    [Serializable]
    public class SpatiallyVariantPsf
    {
        /**
         * The position of the center of the PSF
         */
        public Point Position
        {
            get;
            private set;
        }

        /**
         * The PSF being wrapped
         */
        public PSF Psf
        {
            get;
            private set;
        }

        /**
         * Constructor
         * 
         * @param aPsf
         * The PSF
         * 
         * @param aPosition 
         * The position of the PSF in the underlying picture
         */
        public SpatiallyVariantPsf(PSF aPsf, Point aPosition)
        {
            Psf = aPsf;
            Position = aPosition;
        }
    }
}
