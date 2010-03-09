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

using Microsoft.Win32;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window, INotifyPropertyChanged
    {
        public Window1()
        {
            InitializeComponent();
            this.Zoom = 0.1;
            viewBox.DataContext = this;

            CommandBinding cb = new CommandBinding(ApplicationCommands.Open);
            cb.Executed += new ExecutedRoutedEventHandler(cb_Executed);
            this.CommandBindings.Add(cb);

            CommandBinding cb1 = new CommandBinding(NavigationCommands.Zoom);
            cb1.Executed += new ExecutedRoutedEventHandler(cb1_Executed);
            this.CommandBindings.Add(cb1);
        }

        void cb1_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string level = e.Parameter as string;
            double value = Convert.ToDouble(level);
            Zoom = value;
        }

        void cb_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                theImage.Source = new BitmapImage(new Uri(dlg.FileName));
                viewBox.Width = theImage.Width;
                viewBox.Height = theImage.Height;
            }
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                theImage.Source = new BitmapImage(new Uri(dlg.FileName));
                viewBox.Width = theImage.Width;
                viewBox.Height = theImage.Height;
            }
        }

        private void MenuItem_Zoom25(object sender, RoutedEventArgs e)
        {
            Zoom = 0.25;
        }

        private void MenuItem_Zoom50(object sender, RoutedEventArgs e)
        {
            Zoom = 0.5;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
