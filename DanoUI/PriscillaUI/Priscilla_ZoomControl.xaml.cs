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
    /// Interaction logic for ZoomControl
    /// </summary>
    public partial class ZoomControl : UserControl
    {
        public double ZoomLevel { get => ZoomLevel; set
            {
                ZoomLevel = value;
                ZoomUI_ZoomPercentage.Text = $"{ZoomUI_Slider.Value}%";
            }
        }
        public EventHandler<DanoEventArgs> ZoomLevelChanged { get; set; }
        public ZoomControl()
        {
            InitializeComponent();
        }

        private void ZoomUI_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ZoomLevel = ZoomUI_Slider.Value; 

            DanoEventArgs DEA = new DanoEventArgs();
            DEA.DanoParameters.Add(ZoomLevel);
            ZoomLevelChanged(this, DEA); 
        }
    }
}
