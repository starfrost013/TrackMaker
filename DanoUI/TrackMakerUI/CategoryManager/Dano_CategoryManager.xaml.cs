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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrackMaker.UI
{
    /// <summary>
    /// 
    /// TrackMaker.UI version of the Category Manager.
    /// 
    /// 2020/09/03 [Priscilla build 427]
    /// 
    /// </summary>
    public partial class DanoCategoryManager : UserControl
    {
        public List<string> BasinStrings { get; set; }
        public List<string> CategoryStrings { get; set; }

        /// <summary>
        /// Event arg 0 (bool) - was any change made?
        /// Event arg 1 (string) - change made to this
        /// </summary>
        public EventHandler<DanoEventArgs> CloseHit { get; set; }
        public DanoCategoryManager()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DanoEventArgs DEA = new DanoEventArgs();

            bool HasChanged = false;

            if (Catman_SystemsBox.SelectedIndex != -1)
            {
                HasChanged = true;
                string SelectedCategorySystem = (string)Catman_SystemsBox.Items[Catman_SystemsBox.SelectedIndex];
                DEA.DanoParameters.Add(HasChanged);
                DEA.DanoParameters.Add(SelectedCategorySystem);
            }
            else
            {
                DEA.DanoParameters.Add(HasChanged);
                DEA.DanoParameters.Add(""); 
            }

            RaiseCloseHit(sender, DEA); 
        }

        private protected void RaiseCloseHit(object sender, DanoEventArgs e)
        {
            CloseHit(sender, e); 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;  
        }
    }
}
