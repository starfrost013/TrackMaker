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
        public List<GraphPoint2D> Points { get; set; } // The points of this point. 

        /// <summary>
        /// The settings of this particular GraphLine. 
        /// </summary>
        public GraphLineSettings Settings { get; set; }


        public GraphPoint2D AddPoint(Point Pt)
        {
            GraphPoint2D GP2D = new GraphPoint2D();
            GP2D.Position = Pt;
            Points.Add(GP2D);
            return GP2D;
        }

        public GraphPoint2D GetPointWithId(int Id)
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
