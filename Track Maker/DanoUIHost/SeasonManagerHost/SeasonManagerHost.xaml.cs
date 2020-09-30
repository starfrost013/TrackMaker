﻿using DanoUI;
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
using System.Windows.Shapes;

namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for SeasonManagerHost.xaml
    /// </summary>
    public partial class SeasonManagerHost : Window
    {
        public Project Project { get; set; }
        public SeasonManagerHost(Project Proj)
        {
            InitializeComponent();
            SeasonEd.StormNameList = Proj.SelectedBasin.GetFlatListOfStormNames();
            SeasonEd.UpdateLayout();
            Project = Proj; 
        }

        public void Edit_Click(object sender, DanoEventArgs e)
        {
            Debug.Assert(e.DanoParameters.Count == 1);
            string StormToSelect = (string)e.DanoParameters[0];
            Project.SelectedBasin.SelectStormWithName(StormToSelect); 
        }

        public void Delete_Click(object sender, DanoEventArgs e)
        {
            // pre-globalstate
            Debug.Assert(e.DanoParameters.Count == 1); 
            string DelStorm = (string)e.DanoParameters[0];
            Project.SelectedBasin.RemoveStormWithName(DelStorm); 
            
        }

        public void Rename_Click(object sender, DanoEventArgs e)
        {
            return; // no point throwing an exception when we're going to implement this in less than 24 hours
        }

        public void OK_Hit(object sender, DanoEventArgs e)
        {
            Close(); 
        }
    }
}
