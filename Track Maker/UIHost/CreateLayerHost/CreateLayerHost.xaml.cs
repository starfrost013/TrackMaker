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
    /// Interaction logic for CreateLayerHost.xaml
    /// </summary>
    public partial class CreateLayerHost : Window
    {
        public CreateLayerHost()
        {
            InitializeComponent();
        }

        private void OKHit(object sender, DanoEventArgs e)
        {
            try
            {
                Debug.Assert(e.DanoParameters.Count == 3);

                bool BringToFront = (bool)e.DanoParameters[0];
                bool BringToBack = (bool)e.DanoParameters[1];
                string LayerName = (string)e.DanoParameters[2];

                if (BringToFront && BringToBack)
                {
                    Error.Throw("You cannot select both Bring to Front and Bring to Back options!", "Warning", ErrorSeverity.Warning, 174);
                }
                else
                {
#if PRISCILLA
                    MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
                    MnWindow.CurrentProject.SelectedBasin.AddLayer(LayerName);
                    Close(); 
#else // Dano - globalstate
#endif

                }
            }
            catch (FormatException err)
            {
#if DEBUG
                Error.Throw("Fatal!!", $"An error occurred parsing the DanoParameters for the CreateLayerHost!\n\n{err}", ErrorSeverity.FatalError, 175);
#else
                Error.Throw("Fatal!!", $"An error occurred parsing the DanoParameters for the CreateLayerHost!\n\nThis is likely a bug in the Track Maker.", ErrorSeverity.FatalError, 175);
#endif
            }
        }

    }
}
