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
    /// Interaction logic for AddNewStormHost.xaml
    /// </summary>
    public partial class AddNewStormHost : Window
    {
        public AddNewStormHost()
        {
            InitializeComponent();
            // get the storm names
#if PRISCILLA
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
            StormTypeManager ST2M = MnWindow.ST2Manager;
            List<string> StormNames = ST2M.GetListOfStormTypeNames();
#else
            Basin = GlobalState.GetST2Manager();
            List<string> StormNames = Basin.GetListOfStormTypeNames(); 
#endif
            // PRE BINDING!!!!!!!!!!!!!!
            if (!AddNewStorm.TypeSelect.Setup(StormNames)) Close();  
        }

        /// <summary>
        /// Frontend code for adding a storm...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="DEA"></param>
        private void OKHit(object sender, DanoEventArgs DEA)
        {
            Debug.Assert(DEA.DanoParameters.Count == 3);
#if DANO
            Project Proj = GlobalState.GetCurrentProject();
            Basin Bas = Proj.SelectedBasin; 
#else
            MainWindow MnWindow = new MainWindow();
            Basin Bas = MnWindow.CurrentProject.SelectedBasin;
#endif
            Bas.AddStorm((string)DEA.DanoParameters[0], (string)DEA.DanoParameters[1], (DateTime)DEA.DanoParameters[2]);
            Close(); 
        }
    }
}
