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

        }

        private ImageF baseImage;
        public ImageF BaseImage
        {
            get
            {
                return baseImage;
            }
            set
            {
                baseImage = value;

                ResultImage = ImageF.ConstantImage(baseImage.Width, baseImage.Height, 0.5);
            }
        }

        public ImageF ResultImage
        {
            get;
            set;
        }
    }
}
