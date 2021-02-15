using TrackMaker.Core;
using TrackMaker.Core.Graphing;
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

namespace TrackMaker.UI
{
    /// <summary>
    /// Interaction logic for GraphDisplay.xaml
    /// </summary>
    public partial class GraphDisplay : UserControl
    {
        public StormGraph GraphToDisplay { get; set; }
        public EventHandler<DanoEventArgs> ExitHit { get; set; }
        public GraphDisplay()
        {
            // parametersless constructor required for UC
            //GraphToDisplay = SG;
            InitializeComponent();

            // test code
            GraphToDisplay = new StormGraph();
            GraphToDisplay.Settings.AxesEnabled = true;
            GraphToDisplay.Settings.GridEnabled = true;
            GraphToDisplay.Settings.Smax = new Vector(0, 100);
            GraphToDisplay.Settings.Smin = new Vector(0, 0);
            DrawCurrentGraph();

        }

        private void DrawGraphBoundaries()
        {
            Vector Smin = GraphToDisplay.Settings.Smin;
            Vector Smax = GraphToDisplay.Settings.Smax;

            double DistanceX = 0;
            double DistanceY = 0;

            if (Smin.X < 0)
            {
                if (Smax.X < 0)
                {
                    DistanceX = Math.Abs(Smax.X) + Math.Abs(Smin.X);
                }
                else
                {
                    DistanceX = Math.Abs(Smax.X) - Math.Abs(Smin.X);
                }
            }
            else
            {
                DistanceX = Smax.X - Smin.X;
            }

            if (Smin.Y < 0)
            {
                if (Smax.Y < 0)
                {
                    DistanceY = Math.Abs(Smax.Y) + Math.Abs(Smin.Y);
                }
                else
                {
                    DistanceY = Math.Abs(Smax.Y) - Math.Abs(Smin.Y);
                }
            }

            Debug.Assert(DistanceX >= 0 && DistanceY >= 0);

            double StepFactorX = GraphDisplay_Graph.Width / DistanceX;
            double StepFactorY = GraphDisplay_Graph.Height / DistanceY;

            // used for the numerical drawing for loop
            int DivisorX = Convert.ToInt32(GraphDisplay_Graph.Width / StepFactorX);
            int DivisorY = Convert.ToInt32(GraphDisplay_Graph.Height / StepFactorY);

            Point SDX_Point = new Point(0, 0);

            // draw grid lines and y-axis
            for (int i = 0; i < DivisorY; i++)
            {
                SDX_Point = new Point(0, 0 + DivisorY);

                if (GraphToDisplay.Settings.GridEnabled)
                {

                }

                double CYAxisVal = GraphDisplay_Graph.Height * SDX_Point.Y;
                // tuberculosis block
                TextBlock TB = new TextBlock();
                TB.Text = CYAxisVal.ToString();

                Canvas.SetLeft(TB, SDX_Point.X);
                Canvas.SetTop(TB, SDX_Point.Y);

                GraphDisplay_Graph.Children.Add(TB);
            }
        }

        public void DrawCurrentGraph()
        {
            DrawGraphBoundaries(); 
            //DrawGraphPoints();
        }

        private void DrawGraphPoints()
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
