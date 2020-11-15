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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for LayerControlHost.xaml
    /// </summary>
    public partial class LayerControlHost : UserControl
    {

        public void AddLayer(string Name)
        {
            Layers.AddLayer(Name);
            Layers.UpdateLayout(); 
        }

        public void RemoveLayer(string Name)
        {
            Layers.LayerNames.Remove(Name);
            Layers.UpdateLayout(); 
        }


        public LayerControlHost()
        {
            InitializeComponent();
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
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
            // add remove layer function

            if (MnWindow.CurrentProject != null)
            {
                MnWindow.CurrentProject.SelectedBasin.RemoveLayerWithName((string)e.DanoParameters[0]);
            }

#else 
            LayerManager LH = GlobalState.GetLCH();
#endif
        }

        private void Layers_Loaded(object sender, RoutedEventArgs e)
        {
            Layers.DataContext = Layers;
        }
    }
}
