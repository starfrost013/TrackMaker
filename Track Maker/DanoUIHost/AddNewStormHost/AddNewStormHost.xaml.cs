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
        public AddNewStormHost(DateTime SeasonStartTime)
        {
            InitializeComponent();
            // get the storm names
            
            if (SeasonStartTime == null)
            {
                AddNewStorm.SeasonStartTime = "N/A";
            }
            else
            {
                AddNewStorm.SeasonStartTime = SeasonStartTime.ToString("yyyy-MM-dd HH:mm:ss");
            }

            AddNewStorm.UpdateLayout();
        }

        /// <summary>
        /// Frontend code for adding a storm...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="DEA"></param>
        private void OKHit(object sender, DanoEventArgs DEA)
        {
            Debug.Assert(DEA.DanoParameters.Count == 2);
#if DANO
            Project Proj = GlobalState.GetCurrentProject();
            Basin Bas = Proj.SelectedBasin; 
#else
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;

            string StormName = (string)DEA.DanoParameters[0];
            DateTime StormStartTime = (DateTime)DEA.DanoParameters[1];
            
            Basin Bas = MnWindow.CurrentProject.SelectedBasin;
            List<Storm> FlatList = Bas.GetFlatListOfStorms();

            if (FlatList.Count == 0)
            {
                Bas.SeasonStartTime = StormStartTime;
            }
#endif
            Bas.AddStorm(StormName, StormStartTime);
            Close(); 
        }
    }
}
