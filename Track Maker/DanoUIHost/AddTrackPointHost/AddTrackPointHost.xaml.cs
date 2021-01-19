using TrackMaker.UI;
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
    /// Interaction logic for AddTrackPointHost.xaml
    /// </summary>
    public partial class AddTrackPointHost : Window
    {
        public List<string> StormNameList { get; set; }

        public AddTrackPointHost(List<string> StormNames, Point Position, DateTime SeasonStartTime)
        {
            InitializeComponent();
            AddTrackPointControl.TypeNames = StormNames;
            AddTrackPointControl.MousePosition = Position;
            StormNameList = StormNames;
            AddTrackPointControl.StartTime = SeasonStartTime;
            ToggleAdvancedSettings(false); // bad but will be improved
        }

        public void AdvancedSettingsToggleChanged(object sender, DanoEventArgs DEA)
        {
            Debug.Assert(DEA.DanoParameters.Count == 1); 

            bool AdvancedSettingsToggleState = (bool)DEA.DanoParameters[0];

            ToggleAdvancedSettings(AdvancedSettingsToggleState);

        }

        private void ToggleAdvancedSettings(bool AdvancedSettingsToggleState)
        {
            switch (AdvancedSettingsToggleState)
            {
                case true:
                    Height = 300.331;
                    return;
                case false:
                    Height = 257;
                    return; 
            }
        }

        public void OKHit(object sender, DanoEventArgs DEA)
        {
            Debug.Assert(DEA.DanoParameters.Count == 4);

#if DANO
            Basin CBasin = GlobalState.GetCurrentBasin(); // only valid when project valid
            Storm Sto = CBasin.GetCurrentStorm();
#else
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
            Storm Sto = MnWindow.CurrentProject.SelectedBasin.GetCurrentStorm();

            try
            {
                int Intensity = (int)DEA.DanoParameters[0];
                string Type = (string)DEA.DanoParameters[1];
                Point Position = (Point)DEA.DanoParameters[2];
                int Pressure = (int)DEA.DanoParameters[3];

                Sto.AddNode(Intensity, Type, Position, Pressure, MnWindow.ST2Manager);
                Close();
            }
            catch (InvalidCastException err)
            {
#if DEBUG
                Error.Throw("Warning!", $"Internal error: Cannot convert DanoParameters to their actual types.\n\n{err}", ErrorSeverity.Warning, 403);
#else
                Error.Throw("Warning!", "Internal error: Cannot convert DanoParameters to their actual types.\n\n{err}", ErrorSeverity.Warning, 403); 
#endif
#endif
            }

        }

    }
}
