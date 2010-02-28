using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AstroDeconvolution
{
    /**
     * @brief
     * Base class for Coefficients in Karhunen-Loève %PSF decomposition
     * 
     * These are the coefficients \f$a_i\f$ in the expression
     * \f[
     * P(u, v, x, y) = \sum _ {i = 1} ^ N a_i(u, v, x, y) p_i(x,y)
     * \f]
     * 
     * The class contains methods to 
     */
    public abstract class KLCoefficients
    {
        /**
         * @brief 
         * Get the coefficients expanded to rectangular coordinates
         * 
         * Note that setting this property is private.
         */
        public ImageF Expansion
        {
            get;
            private set;
        }
    }
}
