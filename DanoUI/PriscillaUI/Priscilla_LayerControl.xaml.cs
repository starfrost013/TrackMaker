using Starfrost.UL5.WpfUtil;
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

namespace DanoUI
{
    /// <summary>
    /// Layer User Control 
    /// </summary>
    public partial class LayerControl : UserControl
    {

        public EventHandler<DanoEventArgs> LayerClicked { get; set; }

        /// <summary>
        /// No arguments
        /// </summary>
        public EventHandler<DanoEventArgs> LayerCreated { get; set; }

        /// <summary>
        /// DanoEventArgs
        /// 
        /// Parameter 0 - string - the layer to be deleted [2020-10-20] 
        /// </summary>
        public EventHandler<DanoEventArgs> LayerDeleted { get; set; }

        /// <summary>
        /// DanoEventArgs
        /// 
        /// Parameter 0 - string - the layer name to be disabled [2020-12-07]
        /// </summary>
        public EventHandler<DanoEventArgs> LayerDisabled { get; set; }

        /// <summary>
        /// DanoEventArgs
        /// 
        /// Parameter 0 - string - the layer to be enabled [2020-12-07]
        /// </summary>
        public EventHandler<DanoEventArgs> LayerEnabled { get; set; }

        /// <summary>
        /// DanoEventArgs
        /// 
        /// Parameter 0 - int - amount to move the layer by; negative is up, positive is down. 
        /// </summary>
        public EventHandler<DanoEventArgs> LayerReordered { get; set; }

        /// <summary>
        /// DanoEventArgs
        /// 
        /// Parameter 0 - string - the layer to select
        /// </summary>
        public EventHandler<DanoEventArgs> LayerSelected { get; set; }

#if v21_LayerBinding // change this to use Layer objects in v2.1
        public List<string> LayerNames { get; set; }
#endif
        public LayerControl()
        {
            InitializeComponent();
        }

        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            LayerCreated(sender, new DanoEventArgs());
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DanoEventArgs DEA = new DanoEventArgs();

            if (PriscillaUI_Layers_LayerListView.SelectedIndex != -1)
            {
                DEA.DanoParameters.Add((string)PriscillaUI_Layers_LayerListView.SelectedItem);
                LayerDeleted(sender, DEA);
            }
            else
            {
                MessageBox.Show("Error placeholder");
                return; 
            }
        }

        public void AddLayer(string Lyr)
        {
#if v21_LayerBinding
            LayerNames.Add(Lyr);
#endif
            // SHITTY HACK HITTY HACK!!!!
            PriscillaUI_Layers_LayerListView.Items.Add(Lyr);
            PriscillaUI_Layers_LayerListView.SelectedIndex = PriscillaUI_Layers_LayerListView.Items.Count - 1;
            //todo: SET STATE WHEN ENABLED
            // END SHITTY HACK
        }

        /// <summary>
        /// On layer checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PriscillaUI_Layers_LayerListView_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox ChkBox = GetCheckBox(e);

            if (ChkBox == null)
            {
                MessageBox.Show("TEMP: Failed to acquire CheckBox!");
                return;
            }

            bool IsLayerEnabled = (bool)ChkBox.IsChecked;

            DanoEventArgs DEA = new DanoEventArgs(); 
            string SelItem = (string)PriscillaUI_Layers_LayerListView.SelectedItem;

            DEA.DanoParameters.Add(SelItem);

            // temporary code
            if (!IsLayerEnabled)
            {
                LayerDisabled(this, DEA);
            }
            else
            {
                LayerEnabled(this, DEA);
            }
        }

        private CheckBox GetCheckBox(RoutedEventArgs e)
        {

            CheckBox CB = (CheckBox)e.OriginalSource;
            return CB; 
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e) => TriggerReorderedEvent((string)PriscillaUI_Layers_LayerListView.SelectedItem, -1);

        private void MoveDown_Click(object sender, RoutedEventArgs e) => TriggerReorderedEvent((string)PriscillaUI_Layers_LayerListView.SelectedItem, 1); 


        private void TriggerReorderedEvent(string Name, int Amount)
        {
            DanoEventArgs DEA = new DanoEventArgs();
            DEA.DanoParameters.Add(Name); 
            DEA.DanoParameters.Add(Amount);
            LayerReordered(this, DEA);
        }
    }
}