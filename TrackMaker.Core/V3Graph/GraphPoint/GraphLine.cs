using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Track_Maker.Graphing
{
    /// <summary>
    /// 3.0 (Dano) graph line
    /// </summary>
    public class GraphLine
    {
        public List<GraphPoint2D> Points { get; set; } // The points of this point. 
        public Color Color { get; set; }
 
        /// <summary>
        /// Name of the axis
        /// </summary>
        public string Name { get; set; }
        // 2D graphing only
        public Vector Scale { get; set; }

        public void AddPoint(Point Pt)
        {
            GraphPoint2D GP2D = new GraphPoint2D();
            GP2D.Position = Pt;
            Points.Add(GP2D);
        }

        public void RemovePointWithId(int Id) => Points.RemoveAt(Id);

        public void SetScale(double X, double Y) => Scale = new Vector(X, Y); 
    }
}
