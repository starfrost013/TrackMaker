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
            Layers.AddLayer(Name);
            Layers.UpdateLayout(); 
        }


        public void RemoveLayer(string Name)
        {
#if v21_LayerBinding
            Layers.LayerNames.Add(Name);
#else
            Layers.PriscillaUI_Layers_LayerListView.Items.Remove(Name); 
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

            // no DEA
            CreateLayerHost CLH = new CreateLayerHost();
            CLH.Owner = Application.Current.MainWindow;
            CLH.Show();
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
            Debug.Assert(e.DanoParameters.Count == 1);

            string LayerName = (string)e.DanoParameters[0];

            Basin SBasin = MnWindow.CurrentProject.SelectedBasin;

            SBasin.DisableLayerWithName(LayerName);

            return;
        }

        private void Lyr_Enabled(object sender, DanoEventArgs e)
        {

            Debug.Assert(e.DanoParameters.Count == 1);

            string LayerName = (string)e.DanoParameters[0];

            Basin SBasin = MnWindow.CurrentProject.SelectedBasin;

            SBasin.EnableLayerWithName(LayerName);

            return; 
        }

        private void Lyr_Reordered(object sender, DanoEventArgs e)
        {
#if PRISCILLA
#else // Dano: Use the Glue API
#endif
            MnWindow.StopTimer(); 
            Debug.Assert(e.DanoParameters.Count == 2);

            string LayerName = (string)e.DanoParameters[0];
            int Amount = (int)e.DanoParameters[1];

            Basin SBasin = MnWindow.CurrentProject.SelectedBasin;

            Layer Lyr = SBasin.GetLayerWithName(LayerName);

            Lyr.ZIndex += Amount;

            // Prevent a "collection changed, can't enumerate" invalidoperationexception
            MnWindow.StartTimer();
        }

        private void Layers_Loaded(object sender, RoutedEventArgs e)
        {
            Layers.DataContext = Layers;
        }

        private void Lyr_Selected(object sender, DanoEventArgs e)
        {

            // not the best way to handle this at all
            MnWindow.StopTimer();
            Debug.Assert(e.DanoParameters.Count == 1);

            // silently ail 
            List<string> SelectedList_Added = (List<string>)e.DanoParameters[0];

            // SelectionMode is explicitly set to Single on the layercontrol,
            // so this should be impossible
            Debug.Assert(SelectedList_Added.Count == 1);

            Basin SBasin = MnWindow.CurrentProject.SelectedBasin;

            string LayerSelected = SelectedList_Added[0];

            SBasin.SelectLayerWithName(LayerSelected);

            MnWindow.StartTimer(); 
        }

    }
}
