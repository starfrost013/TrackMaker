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

        /// <summary>
        /// The scale of this graph.
        /// </summary>
        public Vector Scale { get; set; }

    }
}
