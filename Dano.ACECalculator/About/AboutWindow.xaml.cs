﻿using System;
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
using System.Windows.Shapes;

namespace Dano.ACECalculator
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
            FileVersionInfo FileVersion = FileVersionInfo.GetVersionInfo(Assembly.Location); // get this program's location
            Version.Text = $"Version 1.5.255.4 (for Track Maker version {FileVersion.ProductVersion}).";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            AbtWindow.Close();
        }

        private void Version_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Version.Text = "lightning bolt";
        }

        // new in v1.4: actually make this work - 2020-04-08
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        }
    }
}
