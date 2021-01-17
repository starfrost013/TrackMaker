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
    /// Interaction logic for Priscilla_RenameLayer.xaml
    /// </summary>
    public partial class RenameLayer : UserControl
    {

        /// <summary>
        /// The previous name of the selected layer.
        /// </summary>
        public string PreviousName { get; set; }

        /// <summary>
        /// DanoEventArgs
        /// 
        /// Parameter 0     string      The new layer name.
        /// </summary>
        public EventHandler<DanoEventArgs> OKHit { get; set; }
        
        public RenameLayer()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Setup();
        }

        private void Setup()
        {
            RenameLayer_NameBox.DataContext = this;
            UpdateLayout(); // required?
        }

        private void RenameLayer_OKButton_Click(object sender, RoutedEventArgs e)
        {
            string NewLayerName = RenameLayer_NameBox.Text;
            DanoEventArgs DEA = new DanoEventArgs();
            DEA.DanoParameters.Add(NewLayerName);
            OKHit(this, DEA);
        }
    }
}
