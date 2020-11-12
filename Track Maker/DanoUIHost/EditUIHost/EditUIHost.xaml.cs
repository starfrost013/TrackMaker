using DanoUI;
using System;
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
    /// Interaction logic for EditUIHost.xaml
    /// </summary>
    public partial class EditUIHost : Window
    {
        public Project Proj { get; set; }
        public string StoName { get; set; }
        public EditUIHost(Project Proj, string StormName)
        {
            InitializeComponent();
            Init(Proj, StormName); 
        }

        private protected void Init(Project Project, string StormName)
        {
            Proj = Project;
            EditUI.StormName = StormName;
            EditUI.UpdateLayout(); 
            StoName = StormName;
        }

        public void Done_Hit(object sender, DanoEventArgs DEA)
        {
            Proj.SelectedBasin.RenameStormWithName(StoName, EditUI.EditStorm_EditNameBox.Text);
            Close(); 
        }
    }
}
