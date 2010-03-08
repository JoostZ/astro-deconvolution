using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AstroDeconvolution
{
    /**
     * @brief 
     * Interface that allows Convolution into an ImageF
     */
    public interface IConvolutable
    {
        /**
         * @brief
         * Convolute the object with the ImageF
         * 
         * @param image The image to convolute with
         * 
         * @return The result of the convlution
         */
        ImageF Convolute(ImageF image);

        ImageF ConvoluteTranspose(ImageF image);
    }
}
