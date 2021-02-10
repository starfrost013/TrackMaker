using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TrackMaker.Core.Graphing
{

    /// <summary>
    /// 2021-01-25          Iris v692
    /// 
    /// Graphing
    /// 
    /// Graph Settings
    /// 
    /// used for setting the parameters of the graph
    /// </summary>
    public class GraphSettings
    {
        /// <summary>
        /// Axes enabled
        /// </summary>
        public bool AxesEnabled { get; set; }

        /// <summary>
        /// Graph grid enabled
        /// </summary>
        public bool GridEnabled { get; set; }

        /// <summary>
        /// Graph grid size [X,Y]
        /// </summary>
        public Vector GridSize { get; set; }

        /// <summary>
        /// The graph type used by this object.
        /// </summary>
        public GraphType Type { get; set; }

        private Vector _smin; 

        /// <summary>
        /// Thetopleft point of this graph of this graph.
        /// </summary>

        public Vector Smin 
        {   get
            {
                return _smin; 
            }
            set
            {
                if (value.X > Smax.X || value.Y > Smax.Y)
                {
                    Error.Throw("Error", "GS: Fatal graphing error: GraphSettings.Smin < Smax!", ErrorSeverity.FatalError, 418);
                }
                else
                {
                    _smin = value; 
                }
            }
        }
        private Vector _smax { get; set; }
        public Vector Smax 
        {   get
            {
                return _smax;
            }
            set
            {
                if (value.X < Smin.X || value.Y < Smin.Y)
                {
                    Error.Throw("Error", "GS: Fatal graphing error: GraphSettings.Smin < Smax!", ErrorSeverity.FatalError, 418);
                }
                else
                {
                    _smax = value; 
                }
            }
        }

    }
}
