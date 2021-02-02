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
            ToggleAdvancedApplicationSettings(false); // bad but will be improved
        }

        private void OnAdvancedSettingsToggleChanged(object sender, DanoEventArgs DEA)
        {
            Debug.Assert(DEA.DanoParameters.Count == 1); 

            bool AdvancedApplicationSettingsToggleState = (bool)DEA.DanoParameters[0];

            ToggleAdvancedApplicationSettings(AdvancedApplicationSettingsToggleState);

        }

        private void ToggleAdvancedApplicationSettings(bool AdvancedApplicationSettingsToggleState)
        {
            switch (AdvancedApplicationSettingsToggleState)
            {
                case true:
                    AddTrackPointControl.TypeOK.Margin = new Thickness(598, 220, 0, 0);
                    Height = 330.331;

                    return;
                case false:
                    AddTrackPointControl.TypeOK.Margin = new Thickness(598, 154, 0, 0);
                    Height = 237;
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
