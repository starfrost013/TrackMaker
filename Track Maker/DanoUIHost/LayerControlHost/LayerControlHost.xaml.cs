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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for LayerControlHost.xaml
    /// </summary>
    public partial class LayerControlHost : UserControl
    {

#if PRISCILLA
        public MainWindow MnWindow { get; set; }
#endif
        /// <summary>
        /// RESTORE BINDINGS ONCE THREADING ISSUES FIGURED OUT
        /// </summary>
        /// <param name="Name"></param>
        public void AddLayer(string Name)
        {
            Layers.AddLayer(Name, true);
            Layers.UpdateLayout(); 
        }


        public void RemoveLayer(string Name)
        {
#if v21_LayerBinding
            Layers.LayerNames.Add(Name);
#else
            Layers.PriscillaUI_Layers_LayerListView.Items.Remove(Name);
            Basin CBasin = MnWindow.CurrentProject.SelectedBasin;

            CBasin.RemoveLayerWithName(Name); 
#endif
            Layers.UpdateLayout(); 
        }

        /// <summary>
        /// END RESTORE BINDING ONCE THREADING ISSUES FIGURED OUT
        /// </summary>

        public LayerControlHost()
        {
            InitializeComponent();
            MnWindow = (MainWindow)Application.Current.MainWindow; 
        }

        public void Lyr_Created(object sender, DanoEventArgs e)
        {
            //MnWindow.StopTimer();
            // no DEA
            CreateLayerHost CLH = new CreateLayerHost();
            CLH.Owner = Application.Current.MainWindow;
            CLH.Show();
            //MnWindow.StartTimer(); 
        }

        public void Lyr_Deleted(object sender, DanoEventArgs e)
        {
#if PRISCILLA

            Debug.Assert(e.DanoParameters.Count == 1); 

            string LayerName = (string)e.DanoParameters[0];
            
            if (MnWindow.CurrentProject != null)
            {
                
                if (LayerName == "Background")
                {
                    // pending for when we move error handling to UL5
                    Error.Throw("Warning!", "You cannot delete the background layer!", ErrorSeverity.Warning, 227);
                    return; 
                }

#if v21_LayerBinding
                Layers.LayerNames.Remove(LayerName);
#else
                Layers.PriscillaUI_Layers_LayerListView.Items.Remove(LayerName);
#endif
                MnWindow.CurrentProject.SelectedBasin.RemoveLayerWithName(LayerName);
            }

#else
                LayerManager LH = GlobalState.GetLCH();
#endif
        }

        // don't throw an exception as ew could crash
        private void Lyr_Disabled(object sender, DanoEventArgs e)
        {
            //MnWindow.StopTimer();
            Debug.Assert(e.DanoParameters.Count == 1);

            string LayerName = (string)e.DanoParameters[0];

            Basin SBasin = MnWindow.CurrentProject.SelectedBasin;

            SBasin.DisableLayerWithName(LayerName);

            //MnWindow.StartTimer(); 
            return;


        }

        private void Lyr_Enabled(object sender, DanoEventArgs e)
        {
            //MnWindow.StopTimer();

            Debug.Assert(e.DanoParameters.Count == 1);

            string LayerName = (string)e.DanoParameters[0];

            Basin SBasin = MnWindow.CurrentProject.SelectedBasin;

            SBasin.EnableLayerWithName(LayerName);

            //MnWindow.StartTimer();
            return; 
        }

        private void Lyr_Reordered(object sender, DanoEventArgs e)
        {
#if PRISCILLA
#else // Dano: Use the Glue API
#endif
            //MnWindow.StopTimer(); 
            Debug.Assert(e.DanoParameters.Count == 2);

            string LayerName = (string)e.DanoParameters[0];
            int Amount = (int)e.DanoParameters[1];

            Basin SBasin = MnWindow.CurrentProject.SelectedBasin;

            Layer Lyr = SBasin.GetLayerWithName(LayerName);

            Lyr.ZIndex += Amount;

            // Prevent a "collection changed, can't enumerate" invalidoperationexception
            //MnWindow.StartTimer();
        }

        private void Layers_Loaded(object sender, RoutedEventArgs e)
        {
            Layers.DataContext = Layers;
        }

        private void Lyr_Renamed(object sender, DanoEventArgs e)
        {
            Debug.Assert(e.DanoParameters.Count == 2);

            string LayerToRename = (string)e.DanoParameters[0];

            // showdialog to prevent closing of mainwindow while still open
            RenameLayerHost RLH = new RenameLayerHost(LayerToRename);
            RLH.Owner = Application.Current.MainWindow;
            RLH.ShowDialog();

            Layers.RenameLayer(LayerToRename, GetNewName((int)e.DanoParameters[1]));
            
        }

        /// <summary>
        /// A total hack to get the new name of the layer after it's already been renamed.
        /// In V3 everything will be binded to GlobalState, GlueAPI etc so this will not be necessary.
        /// </summary>
        /// <param name="OldName">The old name</param>
        /// <returns>The new name.</returns>
        private string GetNewName(int LayerIndex)
        {
#if PRISCILLA
            Basin CurrentProject = MnWindow.CurrentProject.SelectedBasin;
#else
            Project CurrentProject = GlobalState.GetCurrentProject();
#endif

            // this should never happen...most of this is terrible but will be fixed in v3
            Debug.Assert(CurrentProject.Layers.Count > 0 && (CurrentProject.Layers.Count - 1) <= LayerIndex);

            Layer Lay = CurrentProject.Layers[LayerIndex];

            return Lay.Name;
        }

        private void Lyr_Selected(object sender, DanoEventArgs e)
        {

            // not the best way to handle this at all
            // MnWindow.StopTimer();
            Debug.Assert(e.DanoParameters.Count == 1);

            // silently ail 
            List<string> SelectedList_Added = (List<string>)e.DanoParameters[0];

            // SelectionMode is explicitly set to Single on the layercontrol,
            // so this should be impossible
            
            if (SelectedList_Added.Count < 1)
            {
                // selection did not really change
                return;
            }

            Basin SBasin = MnWindow.CurrentProject.SelectedBasin;

            string LayerSelected = SelectedList_Added[0];

            SBasin.SelectLayerWithName(LayerSelected);

            //MnWindow.StartTimer(); 
        }

        public void ClearLayers() => Layers.ClearLayers(); 
    }
}
