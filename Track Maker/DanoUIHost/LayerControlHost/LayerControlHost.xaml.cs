﻿using DanoUI; 
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

            Debug.Assert(e.DanoParameters.Count == 1); 

            string LayerName = (string)e.DanoParameters[0];
            
            if (MnWindow.CurrentProject != null)
            {
                
                if (LayerName == "Background")
                {
                    // pending for when we move error handling to UL5
                    MessageBox.Show("You cannot delete the background!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; 
                }

                MnWindow.CurrentProject.SelectedBasin.RemoveLayerWithName(LayerName);
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
