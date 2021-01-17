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
    /// Interaction logic for CreateLayer.xaml
    /// </summary>
    public partial class CreateLayer : UserControl
    {
        
        public EventHandler<DanoEventArgs> BringToFrontHit { get; set; }
        public EventHandler<DanoEventArgs> BringToBackHit { get; set; }

        /// <summary>
        /// DanoEventArgs
        /// 
        /// DanoParameters:
        /// 
        /// 0 - bool - is bring to front clicked?
        /// 
        /// 1 - bool - is bring to back clicked?
        /// 
        /// 2 - string - the layer name to create
        /// </summary>
        public EventHandler<DanoEventArgs> OKHit { get; set; }
        public CreateLayer()
        {
            InitializeComponent();
        }

        private void CreateLayer_OKButton_Click(object sender, RoutedEventArgs e)
        {
            DanoEventArgs DEA = new DanoEventArgs();

            DEA.DanoParameters.Add(CreateLayer_MoveToFront.IsChecked);
            DEA.DanoParameters.Add(CreateLayer_MoveToBack.IsChecked);
            DEA.DanoParameters.Add(CreateLayer_NameBox.Text);
            // error checking goes here

            OKHit(this, DEA);
        }
    }
}
