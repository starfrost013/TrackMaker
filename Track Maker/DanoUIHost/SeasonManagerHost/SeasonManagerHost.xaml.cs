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

namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for SeasonManagerHost.xaml
    /// </summary>
    public partial class SeasonManagerHost : Window
    {
        public Project Project { get; set; }
        public SeasonManagerHost(Project Proj, List<string> BasinNames)
        {
            InitializeComponent();
            SeasonEd.StormNameList = BasinNames;
            SeasonEd.UpdateLayout();
            Project = Proj; 
        }

        public void Delete_Click(object sender, DanoEventArgs e)
        {
            Debug.Assert(e.DanoParameters.Count == 1); 
            string DelStorm = (string)e.DanoParameters[0];
            Project.SelectedBasin.RemoveStormWithName(DelStorm); 
            // Temp - pre-globalstate
            
        }
    }
}
