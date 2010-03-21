/**
 * @mainpage Correction of astro photos by deconvolution 
 * \section introduction Introduction
 * Stars are in principle point sources. However, if we take a picture
 * of stars, we find that the star images have a finite extension. This
 * is caused by the atmosphere and imperfections of the telescope and
 * recording media. This spreading of the star image can be described 
 * by <i>convolution</i>:
 * \f[I(x, y) = \int _{-\infty} ^ {+\infty}  \int _{-\infty} ^ {+\infty} PSF(x', y') S(x-x', y-y')dx' dy'\f]
 * where \f$S(x,y)\f$ is the real star image, \f$PSF(x,y)\f$ is the <I>Point Spread Function</I> and 
 * \f$I(x,y)\f$ is the resulting image.
 * 
 * Programs like IRIS allow you to do <I>deconvolution</I>, i.e., to reverse the process described above and to 
 * get an image that is a better representation of \f$S(x,y)\f$. However, this method is restricted to %%PSF that 
 * are position independent. Some of the distortions that can occur in image recording have a %%PSF that is position
 * dependent. For instance, coma is a deformation that increases to the edge of the picture. Another example is a
 * picture taken with a telescope mount where the polar axis is not perfectly aligned, causing stars to be recorded
 * as circle segments.
 * 
 * This program is based on the article
 * <a href=http://arxiv.org/abs/astro-ph/0208247>Deconvolution with a spatially-variant PSF</a>.
 * 
 * \section theory The theory
 * 
 * A standard way of doing deconvolution is the <i>Lucy-Richardson Deconvolution Algorithm</i>. This algorithm starts with 
 * an original estimate of the star image \f$P(x,y)\f$. This image is then convoluted with the known %PSF and the result is 
 * compared with the actual image \f$I(x,y)\f$. Based on the difference a new estimate of the star image is generated and
 * these steps are repeated until the two images compare well.
 * 
 * By far the fastest way to do the convolutions is by means of the Fast Foerier Transform (FFT). However, this only works 
 * the %PSF is constant over the area of the image.
 * 
 * As described in <a href=http://arxiv.org/abs/astro-ph/0208247>Deconvolution with a spatially-variant PSF</a>
 * it is possible to generate a set of orthogonal base %PSF, analogous to generating a set of orthogonal base
 * vectors in an N-dimensional vector space, in such a way that each %PSF can be expressed as a lineair combination
 * of these base %PSF. If we write the base %PSF, that are independent of the position in the image as \f$P_i(x,y\f$, then we can write
 * \f[
 * P(u, v,x,y) = \sum _ {i = 1} ^ N a_i(u, v, x,y) P_i(x,y)
 * \f]
 * where \f$u\f$ and \f$v\f$ are the coordinates in the image and \f$x\f$ and \f$y\f$ are trhe coordinates in the %PSF.
 * 
 * With this expression we can write the convolution of the star image with the spatially variant %PSF as
 * \f[
 * I (x,y) = \sum _{i = 1} ^ N  \int _{-\infty} ^ {+\infty}  \int _{-\infty} ^ {+\infty}  S(u, v) a_i (u, v)
 *  p_i (x -u, y - v)du dv
 * \f]
 * 
 * Each of the terms in this formula is now a convolution with a %PSF that is spatially invariant and and can therefore
 * be calculated with FFT.
 * 
 * This program handles correction of (astro) photos by deconvolution with a spatially-variant %PSF.
 * 
 * \section algorithm The Algorithm
 * The first step is to determine the %PSF as function of the position in the image. This program does
 * not use theoretical formluas ot parameterized functions for the %PSF. In stead it assumes that the pixels
 * around a star in the recorded image <i>is</i> the %PSF. This is done in class %PSF.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AstroDeconvolution
{
    /**
     * @brief
     * Richardson-Lucy deconvolution
     * 
     * As described on \ref index, we assume that the actulal image  \f$I\f$is a 
     * convolution of the true star field \f$S\f$ with a Point Spread Function
     * \f$P\f$. We want to de-convolve the measured image with the known %PSF to get a
     * better quality image.
     * 
     * Richardson-Lucy de-convolution solves this by an iterative procedure. From an estimate
     * \f$S_n\f$ of the star image we calculate an estimate of the image \f$I_n\f$ by convolution:
     * \f[
     * I_n = S_n \star P
     * \f]
     * A correction image is then estimated by convolving a ratio of the true
     * to estimated image by the transpose of the %PSF,
     * \f[
     * C_n = \left( I \over I_n \right ) \star P^T
     * \f]
     * This image is then used to create a new estimate of the star image,
     * \f[
     * S_{n+1} = C_n S_n
     * \f]
     * This is then iterated until the \f$I_n\f$ is close to \f$I\f$.
     */
    public class RichardsonLucyDeconvolution
    {
        /**
         * @brief
         * Constructor
         * 
         * @param image
         * The image 
         * @param psf
         * The %PSF to de-convolve
         * 
         * Note that it is not important to know about the %PSF, just that
         * it is an object that can convolve itself withan image
         */
        public RichardsonLucyDeconvolution(ImageF image, IConvolutable psf)
        {
            Image = image;
            Psf = psf;
            Sn = ImageF.ConstantImage(image.Width, image.Height, 0.5);
        }

        private ImageF Image
        {
            get;
            set;
        }

        private IConvolutable Psf
        {
            get;
            set;
        }

        /**
         * @brief
         * Get the current iteration of the image
         * 
         * @note The set operation is private
         */
        public ImageF Sn
        {
            get;
            private set;
        }

        /**
         * @brief
         * Perform a single iteration
         * 
         * @return The new estimate of the image
         */
        public ImageF Iterate()
        {
            ImageF In = Psf.Convolute(Sn);
            ImageF Cn = Psf.ConvoluteTranspose(Image / In);
            Sn *= Cn;
            return Sn;
        }

    }
}
