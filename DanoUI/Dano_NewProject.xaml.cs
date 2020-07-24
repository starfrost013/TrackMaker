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

namespace DanoUI
{
    /// <summary>
    /// Interaction logic for Dano_NewProject.xaml
    /// </summary>
    public partial class Dano_NewProject : UserControl
    {
        public EventHandler<DanoEventArgs> NewProjectCreated { get; set; }
        public List<string> DanoBasinList { get; set; }
        public Dano_NewProject(List<string> DBL)
        {
            DanoBasinList = DBL;
            InitializeComponent();
        }

        private void Dano_UI_CreateProject_Create_Click(object sender, RoutedEventArgs e)
        {
            DanoEventArgs DEA = new DanoEventArgs();
            DEA.DanoParameters.Add(Dano_UI_CreateProject_NameBox.Text);
            DEA.DanoParameters.Add(Dano_UI_CreateProject_InitialBasinBox.Text);
            NewProjectCreated.Invoke(sender, DEA); 
        }
    }
}
