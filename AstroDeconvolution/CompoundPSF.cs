using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AstroDeconvolution
{
    /**
     * @brief 
     * Spatially Variant PSF, with Karhunen-Loève decomposition
     */
    public class CompoundPSF : IConvolutable
    {
        /**
         * @brief
         * Constructor
         */
        public CompoundPSF()
        {
        }

        /**
         * @brief 
         * Construct from a list of PSF
         * 
         * @param psf The list to contruct the CompoundPSF from
         * @param aPrototype 
         * Prototype of the coefficients to use in decomposition
         */
        public void Construct(List<SpatiallyVariantPsf> psf, KLCoefficients aPrototype)
        {
        }

        #region IConvolutable Members

        public ImageF Convolute(ImageF image)
        {
            throw new NotImplementedException();
        }

        public ImageF ConvoluteTranspose(ImageF image)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
