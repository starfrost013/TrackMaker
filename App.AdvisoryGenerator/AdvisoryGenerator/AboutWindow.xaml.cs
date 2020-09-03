using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace Dano.AdvisoryGenerator
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            Assembly Assembly = Assembly.GetEntryAssembly();
            FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(Assembly.Location);
            Version.Text = $"Version 1.0.73.4 (for Cosmo's Track Maker {FVI.ProductVersion})";
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            e.Handled = true;
        }
    }
}
