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

namespace DanoUI
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutUI : UserControl
    {
        public EventHandler<RequestNavigateEventArgs> OnHyperlinkHit { get; set; }
        public EventHandler<RoutedEventArgs> OnOKButtonHit { get; set; }

        public AboutUI()
        {
            InitializeComponent();
        }

        private void Setup()
        {
            FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
#if DANO // Track Maker 3.0 Debug
#if DEBUG
            starfrostTrack_Version.Text = $"Version 3.0 Alpha (Dano v{FVI.ProductVersion} Debug)";
#else // Track Maker 3.0 Release
            starfrostTrack_Version.Text = $"Version 3.0 Alpha (Dano v{FVI.ProductVersion})";
#endif

#elif PRISCILLA

#if DEBUG
            //starfrostTrack_Version.Text = $"Version 2.0 Alpha (Priscilla v{FVI.ProductVersion}) (Debug)";

            starfrostTrack_Version.Text = $"Version 2.0 Beta Release (Priscilla v{FVI.ProductVersion} Debug)";
            starfrostTrack_Copyright_HHW.Text = $"";
            starfrostTrack_Copyright.Text = "This version of the Track Maker is a beta release intended for evaluation purposes only and is not for operational usage. © 2019-2020 starfrost.";

#else
            //starfrostTrack_Version.Text = $"Version 2.0 Alpha (Priscilla v{FVI.ProductVersion})";
            starfrostTrack_Version.Text = $"Version 2.0 Beta Release (Priscilla v{FVI.ProductVersion} (Debug)";
            starfrostTrack_Copyright_HHW.Text = $"";
            starfrostTrack_Copyright.Text = "This version of the Track Maker is a beta release intended for evaluation purposes only and is not for operational usage. © 2019-2020 starfrost.";
#endif

#else
            starfrostTrack_Version.Text = $"Version {FVI.ProductVersion}";
#endif

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Setup(); 
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            OnHyperlinkHit(this, e); 
        }

        private void starfrostTrack_OKButton_Click(object sender, RoutedEventArgs e)
        {
            OnOKButtonHit(this, e); 
        }
    }
}
