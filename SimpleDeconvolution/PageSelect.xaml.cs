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
using System.ComponentModel;


using System.Globalization;
using Microsoft.Win32;

using AstroDeconvolution;

namespace SimpleDeconvolution
{
    /// <summary>
    /// Interaction logic for PageSelect.xaml
    /// </summary>
    public partial class PageSelect : Page, INotifyPropertyChanged
    {
        public PageSelect()
        {
            InitializeComponent();

            this.Zoom = 1.0;
            viewBox.DataContext = this;

            CommandBinding cb = new CommandBinding(ApplicationCommands.Open);
            cb.Executed += new ExecutedRoutedEventHandler(cb_Executed);
            this.CommandBindings.Add(cb);

            CommandBinding cb1 = new CommandBinding(NavigationCommands.Zoom);
            cb1.Executed += new ExecutedRoutedEventHandler(cb1_Executed);
            this.CommandBindings.Add(cb1);

            ImageF image = ImageF.RandomPixelImage(1000, 1000, 100);
            Picture = new ImageFWrapper(image);

            theImage.Source = Picture.Bitmap;
            viewBox.Width = theImage.Width;
            viewBox.Height = theImage.Height;

            Psf = PSF.SymmetricGaussian(5.5);
        }

        ImageFWrapper Picture
        {
            get;
            set;
        }

        PSF _psf;
        PSF Psf
        {
            get
            {
                return _psf;
            }
            set
            {
                _psf = value;
            }
        }

        void cb1_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string level = e.Parameter as string;
            double value = Convert.ToDouble(level, new CultureInfo("en-us"));
            Zoom = value;
        }

        void cb_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                Picture = new ImageFWrapper(ImageF.FromFile(dlg.FileName));

                theImage.Source = Picture.Bitmap;
                viewBox.Width = theImage.Width;
                viewBox.Height = theImage.Height;
            }
        }

        static BitmapSource ToBitmap(ImageF image)
        {
            System.Drawing.Color[,] color = image.ToRawImage();
            return ToBitmap(color);
        }

        private static BitmapSource ToBitmap(System.Drawing.Color[,] color)
        {
            int width = color.GetLength(0);
            int height = color.GetLength(1);
            PixelFormat pf = PixelFormats.Rgb24;
            int rawStride = width * ((pf.BitsPerPixel + 7) / 8);
            byte[] rawImage = new byte[rawStride * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    setpixel(ref rawImage, x, y, rawStride, color[x, y]);
                }
            }

            BitmapSource result = BitmapSource.Create(width, height, 96, 96, pf, null, rawImage, rawStride);
            return result;
        }

        private static void setpixel(ref byte[] bits,
                    int x, int y, int stride, System.Drawing.Color c)
        {
            bits[x * 3 + y * stride] = c.R;
            bits[x * 3 + y * stride + 1] = c.G;
            bits[x * 3 + y * stride + 2] = c.B;
        }

        private double _zoom;
        public double Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Zoom"));
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        private void viewBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(theImage);
            Psf = PSF.FromBitmap(Picture.Image, new System.Drawing.Point((int)pos.X, (int)pos.Y));
            imgPsfPreview.Source = ToBitmap(Psf.ToRawImage());
        }

        private void btnDeconvolve_Click(object sender, RoutedEventArgs e)
        {
            ImageF img = Psf.Convolute(Picture.Image);

            PageDeconvolve page = new PageDeconvolve(new ImageFWrapper(img), Psf);
            this.NavigationService.Navigate(page);
        }

        private void slider1_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void DockPanel_Loaded(object sender, RoutedEventArgs e)
        {

            imgPsfPreview.Source = ToBitmap(Psf.ToRawImage());
        }

    }
}
