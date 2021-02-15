using TrackMaker.Core.Graphing; 
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
    /// Interaction logic for GraphUI.xaml
    /// </summary>
    public partial class GraphUI : UserControl
    {
        /// <summary>
        /// The current graph.
        /// 
        /// Temp: move to GraphingGlobalState class
        /// </summary>
        public StormGraph CurrentGraph { get; set; }
        public EventHandler<DanoEventArgs> DisplayButtonHit { get; set; }
        public GraphUI()
        {
            InitializeComponent();
        }

        private void GraphUI_DisplayGraphButton_Click(object sender, RoutedEventArgs e)
        {
            DanoEventArgs DEA = new DanoEventArgs();
            DEA.DanoParameters.Add(CurrentGraph);
            DisplayButtonHit(this, DEA);
        }
    }
}
