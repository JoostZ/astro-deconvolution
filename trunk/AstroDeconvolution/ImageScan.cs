using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace AstroDeconvolution
{
    /**
     * @brief
     * Show PSF images in a 'film strip'
     */ 
    class ImageScan : FlowLayoutPanel
    {
        Size _szImage;

        /**
         * Currently selected PSF
         */
        public PsfWrapper PSF
        {
            get;
            private set;
        }

        /**
         * Constructor
         */
        public ImageScan()
        {
            FlowDirection = FlowDirection.LeftToRight;
            WrapContents = false;
            AutoScroll = true;

            using (Graphics grfx = CreateGraphics())
            {
                _szImage = new Size(50, 50);
            }

            Height = _szImage.Height + Font.Height +
                SystemInformation.HorizontalScrollBarHeight;

        }

        List<PsfWrapper> iPsfList = new List<PsfWrapper>();

        /**
         * Adda PSF to the images being shown
         * 
         * @param aPsf The PSF to add
         */
        public void Add(PsfWrapper aPsf)
        {
            iPsfList.Add(aPsf);
            Image psfImage = aPsf.Psf.Bitmap;
            PictureBox picBox = new SelectablePictureBox();
            picBox.Parent = this;
            picBox.Size = _szImage;
            picBox.SizeMode = PictureBoxSizeMode.Zoom;
            picBox.Click += PictureBoxOnClick;
            picBox.Image = psfImage;
        }

        public PsfWrapper GetPsf(int idx)
        {
            return iPsfList[idx];
        }

        private void PictureBoxOnClick(object sender, EventArgs e)
        {
            PictureBox picBox = sender as PictureBox;
            foreach (var psf in iPsfList)
            {
                if (psf.Psf.Bitmap.Equals(picBox.Image))
                {
                    PSF = psf;
                    break;
                }
            }

            OnClick(e);
        }
    }
}
