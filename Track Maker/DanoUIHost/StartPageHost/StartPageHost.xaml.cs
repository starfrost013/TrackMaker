﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for StartPageHost.xaml
    /// </summary>
    public partial class StartPageHost : Window
    {
        public StartPageHost()
        {
            InitializeComponent();
        }
        
        private void Dano_StartPage_Exit(object sender, EventArgs e)
        {
            Close(); 
        }

        //TODO: Types
        private void Dano_CreateTrack(object sender, EventArgs e)
        {
            CreateProjectHost CPH = new CreateProjectHost();
            CPH.ShowDialog();
            Close();
        }
    }
}
