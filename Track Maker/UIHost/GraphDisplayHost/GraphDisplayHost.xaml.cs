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
using System.Windows.Shapes;

namespace TrackMaker.UI
{
    /// <summary>
    /// Interaction logic for GraphDisplayHost.xaml
    /// </summary>
    public partial class GraphDisplayHost : Window
    {

        public GraphDisplayHost(StormGraph SG)
        {
            InitializeComponent();
            GraphDisplay.GraphToDisplay = SG;
        }
    }
}
