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
using System.Windows.Shapes;
using TrackMaker.Core;

namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for DanoBasinSwitcherHost.xaml
    /// </summary>
    public partial class DanoBasinSwitcherHost : Window
    {
        public MainWindow MnWindow { get; set; }
        public DanoBasinSwitcherHost(List<string> XStrList)
        {
            InitializeComponent();
            Dano_BasinSwitcherUserControl.BasinString = XStrList;
            Dano_BasinSwitcherUserControl.UpdateLayout();
            MnWindow = (MainWindow)Application.Current.MainWindow; 
        }

        // Event Handler for    Dano UI BasinSwitcher UserControl Closed event

        public void UC_Closed(object sender, DanoEventArgs e)
        {
            string NewBasinName = (string)e.DanoParameters[0];
            Project Proj = MnWindow.CurrentProject;

            // v650b: i just copied this to amke this work so lol
            // Create the basins

            // clean the UI (VERY TEMP; only use for beta - v605)
            MnWindow.Layers.ClearLayers();

            Basin NewBasin = Proj.GetBasinWithName(NewBasinName);

            Proj.AddBasin(NewBasinName, true);

            // set the UI-side image path.
            MnWindow.ImagePath = NewBasin.ImagePath;

            MnWindow.CurrentProject = Proj;

            UpdateLayout();
            Close();
        }

    }
}
