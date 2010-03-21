using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AstroDeconvolution;

namespace SimpleDeconvolution
{
    /// <summary>
    /// Interaction logic for PageDeconvolve.xaml
    /// </summary>
    public partial class PageDeconvolve : Page
    {
        public PageDeconvolve()
        {
            InitializeComponent();

            viewBase.DataContext = this;
            viewBoxResult.DataContext = this;

        }

        public PageDeconvolve(ImageFWrapper image, PSF psf) : this()
        {
            BaseImage = image;
            Rc = new RichardsonLucyDeconvolution(image.Image, psf);
            ResultImage = new ImageFWrapper(Rc.Sn);
        }

        private RichardsonLucyDeconvolution Rc
        {
            get;
            set;
        }
        private ImageFWrapper baseImage;
        public ImageFWrapper BaseImage
        {
            get
            {
                return baseImage;
            }
            set
            {
                baseImage = value;
                theBase.Source = BaseImage.Bitmap;

                ResultImage = baseImage; // new ImageFWrapper(ImageF.ConstantImage(baseImage.Image.Width, baseImage.Image.Height, 0.5));
                theResult.Source = ResultImage.Bitmap;
            }
        }

        public ImageFWrapper ResultImage
        {
            get;
            set;
        }

        private void btnIterate_Click(object sender, RoutedEventArgs e)
        {
            ResultImage.Image = Rc.Iterate();
            theResult.Source = ResultImage.Bitmap;
        }
    }
}
