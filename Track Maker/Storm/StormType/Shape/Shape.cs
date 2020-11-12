using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Track_Maker
{
    /// <summary>
    /// A shape for a track point / node. 
    /// </summary>
    public class Shape
    {
        /// <summary>
        /// The points of this vertex.
        /// </summary>
        public Polygon VPoints { get; set; }

        /// <summary>
        /// Is this shape filled?
        /// </summary>
        public bool IsFilled { get; set; }

        /// <summary>
        /// Thickness of lines if any
        /// </summary>
        public int LineThickness { get; set; }

        /// <summary>
        /// Radius of this shape
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Shape class constructor
        /// </summary>
        public Shape()
        {
            VPoints = new Polygon();
            VPoints.Points = new PointCollection(); 
        }
    }

}
