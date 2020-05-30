using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
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
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        }

        private void Setup()
        {
            FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
#if DANO
            CosmoTrack_Version.Text = $"Version 2.0 \"Dano\" Milestone 1 ({FVI.ProductVersion})";
#else
            CosmoTrack_Version.Text = $"Version {FVI.ProductVersion}";
#endif


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Setup(); 
        }

        private void CosmoTrack_OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }
    }
}
