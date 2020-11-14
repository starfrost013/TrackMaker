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

namespace Track_Maker.DanoUIHost.AddTrackPointHost
{
    /// <summary>
    /// Interaction logic for AddTrackPointHost.xaml
    /// </summary>
    public partial class AddTrackPointHost : Window
    {
        public List<string> StormNameList { get; set; }
        public AddTrackPointHost(List<string> StormNames, Point Position)
        {
            InitializeComponent();
            AddTrackPointControl.TypeNames = StormNames;
            AddTrackPointControl.MousePosition = Position;
            StormNameList = StormNames;
        }

        public void OKHit(object sender, DanoEventArgs DEA)
        {
            Debug.Assert(DEA.DanoParameters.Count == 3);

#if DANO
            Basin CBasin = GlobalState.GetCurrentBasin(); // only valid when project valid
            Storm Sto = CBasin.GetCurrentStorm();
#else
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
            Storm Sto = MnWindow.CurrentProject.SelectedBasin.GetCurrentStorm();
#endif

            Sto.AddNode((int)DEA.DanoParameters[0], (string)DEA.DanoParameters[1], (Point)DEA.DanoParameters[2]);
            Close();
        }

    }
}
