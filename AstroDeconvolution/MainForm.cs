using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace AstroDeconvolution
{
    
    public partial class MainForm : Form
    {
        private PhotographData photograph;
        private Image iImage;
        private ImageF iImageF;

        public MainForm()
        {
            InitializeComponent();
        }

        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                String theFile = openFileDialog.FileName;
                photograph = new PhotographData(theFile);
                iImage = Bitmap.FromFile(theFile);
                iImageF = ImageF.FromBitmap(iImage as Bitmap);
                this.pictureMain.Size = iImage.Size;
                pictureMain.Image = iImage;
                //this.AutoScrollMinSize = new Size(iImage.Width, iImage.Height);
            }

        }

        //private void panelMain_Paint(object sender, PaintEventArgs e)
        //{
        //    if (iImage != null)
        //    {
        //        panelMain.AutoScroll = true;
        //        using (Graphics gfx = panelMain.CreateGraphics())
        //        {
        //            gfx.DrawImage(iImage,
        //                            AutoScrollPosition.X,
        //                            AutoScrollPosition.Y,
        //                            iImage.Width,
        //                            iImage.Height);
        //        }
        //    }
        //}


        private void pictureMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (iImage != null && InPanel)
            {
                Point pos = e.Location;
                //pos.X -= panelImage.AutoScrollPosition.X;
                //pos.Y -= panelImage.AutoScrollPosition.Y;
                PSF thePsf = PSF.FromBitmap(iImageF, pos);
                PsfWrapper wrapper = new PsfWrapper(thePsf, pos);
                this.imageScan1.Add(wrapper);
                photograph.AddPsf(wrapper);
                picPreviewPsf.Image = thePsf.Bitmap;
            }
        }

        private bool InPanel = false;

        private void pictureMain_MouseEnter(object sender, EventArgs e)
        {
            this.panelImage.Cursor = System.Windows.Forms.Cursors.Cross;
            InPanel = true;
        }

        private void pictureMain_MouseLeave(object sender, EventArgs e)
        {
            InPanel = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "PhotoData|*.pds";
            dlg.DefaultExt = "pds";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                photograph.Save(dlg.FileName);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "PhotoData|*.pds";
            dlg.DefaultExt = "pds";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                photograph = PhotographData.Load(dlg.FileName);

                iImage = Bitmap.FromFile(photograph.Image);
                iImageF = ImageF.FromBitmap(iImage as Bitmap);
                this.pictureMain.Size = iImage.Size;
                pictureMain.Image = iImage;

                foreach (PsfWrapper thePsf in photograph.PSFs)
                {
                    this.imageScan1.Add(thePsf);
                }
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            var r = from pix in photograph.AllPixels select pix.Value;
            HotellingTransform transform = new HotellingTransform(r);
            transform.Transform(r.ElementAt(0));
            var builder = new CovarianceBuilder(photograph);
            Matrix covariance = builder.Create();
            EigenvalueDecomposition eigen = covariance.EigenvalueDecomposition;
            Complex[] eigenValues = eigen.EigenValues;
            Matrix eigenVectors = eigen.EigenVectors;

        }

    }
}