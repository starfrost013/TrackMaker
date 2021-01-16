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
    /// Interaction logic for RenameLayerHost.xaml
    /// </summary>
    public partial class RenameLayerHost : Window
    {
        /// <summary>
        /// Just for easier coding ig? maybe not best practice but yeah
        /// </summary>
        public string PrevName { get; set; }
        public RenameLayerHost(string PreviousName)
        {
            InitializeComponent();
            PrevName = PreviousName;
            RenameLayerControl.PreviousName = PrevName;
        }

        public void OKHit(object sender, DanoEventArgs DEA)
        {
            Debug.Assert(DEA.DanoParameters.Count == 1);

            string NewLayerName = (string)DEA.DanoParameters[0];

            if (NewLayerName == "")
            {
                Error.Throw("Error!", "Please enter a new layer name!", ErrorSeverity.Error, 247);
                return; 
            }

#if PRISCILLA // Priscilla (2.0) / Iris (2.1)
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;

            Basin CurBasin = MnWindow.CurrentProject.SelectedBasin;

            if (!CurBasin.RenameLayerWithName(PrevName, NewLayerName))
            {
                Error.Throw("Fatal Error!", "Attempted to rename nonexistent layer!", ErrorSeverity.Error, 248);
                return;
            }

            Close();

            
#else
            // Glue API layer manager.... (Dano / 3.0) 
#endif
        }
    }
}
