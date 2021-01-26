using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TrackMaker.Core
{
    /// <summary>
    /// Defines a graph line
    /// </summary>
    public class GraphLine
    {
        public List<GraphPoint> Points { get; set; } // The points of this point. 

        /// <summary>
        /// The settings of this particular GraphLine. 
        /// </summary>
        public GraphLineSettings Settings { get; set; }


        public GraphPoint AddPoint(Point Pt)
        {
            GraphPoint GP2D = new GraphPoint();
            GP2D.Settings.Position = Pt;
            Points.Add(GP2D);
            return GP2D;
        }

        public GraphPoint GetPointWithId(int Id)
        {
            if (Id < 0 || Id > (Points.Count - 1))
            {
                return null; // v3: result classes
            }
            else
            {
                return Points[Id];
            }
        }

        public void RemovePointWithId(int Id) => Points.RemoveAt(Id);

    }
}
