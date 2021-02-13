using TrackMaker.Core;
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
    /// Interaction logic for GraphDisplay.xaml
    /// </summary>
    public partial class GraphDisplay : UserControl
    {
        public StormGraph GraphToDisplay { get; set; }
        public EventHandler<DanoEventArgs> ExitHit { get; set; }
        public GraphDisplay(StormGraph SG)
        {
            GraphToDisplay = SG;
            InitializeComponent();
            DrawCurrentGraph();
        }

        public void DrawGraphBoundaries()
        {
            Vector Smin = GraphToDisplay.Settings.Smin;
            Vector Smax = GraphToDisplay.Settings.Smax;
        }

        public void DrawCurrentGraph()
        {
            foreach (GraphLine GraphLine in GraphToDisplay.Lines)
            {
                foreach (GraphPoint GP in GraphLine.Points)
                {
                    Rectangle RSX = new Rectangle();
                    RSX.StrokeThickness = GraphLine.Settings.StrokeThickness;
                    RSX.Fill = new SolidColorBrush(GraphLine.Settings.Colour);
                }
            }
        }
    }
}
