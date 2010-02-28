using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace AstroDeconvolution
{
    /**
     * Wrapper around Point Spread Functions
     * 
     * Mostly used to serialize the PSF
     */
    [Serializable]
    class PsfWrapper
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
        public PsfWrapper(PSF aPsf, Point aPosition)
        {
            Psf = aPsf;
            Position = aPosition;
        }
    }
}
