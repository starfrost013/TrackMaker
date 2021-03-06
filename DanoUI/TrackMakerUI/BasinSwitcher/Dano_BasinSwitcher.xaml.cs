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

/// <summary>
/// TrackMaker.UI Basin Switcher testing
/// 
/// 2020-05-25
/// </summary>

namespace TrackMaker.UI
{
    /// <summary>
    /// Interaction logic for BasinSwitcher.xaml
    /// </summary>
    public partial class BasinSwitcher : UserControl
    {
        public List<string> BasinString { get; set; }

        public EventHandler<DanoEventArgs> CloseHit { get; set; }
        

        // Pass the basin list
        public BasinSwitcher()
        {
            InitializeComponent();


        }

        private void Setup()
        {
            BasinList.DataContext = this; // test
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Setup();     
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)

        {

            if (BasinList.SelectedIndex == -1)
            {
                MessageBox.Show("You must select a basin!");
                return; 
            }

            DanoEventArgs DEA = new DanoEventArgs();
            DEA.DanoParameters = new List<object>();
            DEA.DanoParameters.Add(BasinString[BasinList.SelectedIndex]);
            RaiseCloseEvent(DEA);
            
        }

        private void RaiseCloseEvent(DanoEventArgs e)
        {
            CloseHit(this, e); 
        }
    }
}
