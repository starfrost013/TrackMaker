using TrackMaker.Util.MathUtil;
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

namespace TrackMaker.UI
{
    /// <summary>
    /// Interaction logic for ZoomControl
    /// </summary>
    public partial class ZoomControl : UserControl
    {
        /// <summary>
        /// Breaking news: local man fails at accessors [2020.10.24] 
        /// </summary>
        private double Internal_ZoomLevel { get; set; }

        /// <summary>
        /// Prevent a crash by only calling it the second time...HACK
        /// </summary>
        private bool ControlInitialised { get; set; }
        public double ZoomLevel { get => Internal_ZoomLevel; set
            {
                Internal_ZoomLevel = value;
                double RoundedZoomLevel = MathUtil.RoundNearest(Internal_ZoomLevel, 1); 
                ZoomUI_ZoomPercentage.Text = $"{RoundedZoomLevel}%";
            }
        }
        public EventHandler<DanoEventArgs> ZoomLevelChanged { get; set; }
        public ZoomControl()
        {
            InitializeComponent();
        }

        private void ZoomUI_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!ControlInitialised) return; 
            ZoomLevel = ZoomUI_Slider.Value; 

            DanoEventArgs DEA = new DanoEventArgs();
            DEA.DanoParameters.Add(ZoomLevel);
            ZoomLevelChanged(this, DEA); 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // this is a terrible ugly hack. DO NOT DO THIS!!!! We basically just 
            // ignore setting it the first time. FIGURE OUT A BETTER WAY TO DO THIS! [2.0.501b]
            ControlInitialised = true;
            ZoomLevel = 100; 
        }
    }
}
