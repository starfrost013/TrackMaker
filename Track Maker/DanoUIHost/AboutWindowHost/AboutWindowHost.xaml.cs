using DanoUI; 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation; 
using System.Windows.Shapes;

namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for AboutWindowHost.xaml
    /// </summary>
    public partial class AboutWindowHost : Window
    {
        public AboutWindowHost()
        {
            InitializeComponent();
        }

        private void HyperlinkHit(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }

        private void OnOkButtonHit(object sender, RoutedEventArgs e)
        {
            Close(); 
        }



    }
}
