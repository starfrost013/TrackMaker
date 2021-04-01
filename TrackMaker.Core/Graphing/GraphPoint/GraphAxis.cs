using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Core.Graphing
{
    /// <summary>
    /// Graphing Subsystem (Iris v728 - 2021-02-16)
    /// 
    /// GraphAxis.cs
    /// 
    /// Handles graph axes. Only two supported - X and Y - for now.
    /// </summary>
    public class GraphAxis
    {
        public GraphAxisOrder AxisOrder { get; set; }
        /// <summary>
        /// Is this axis enabled?
        /// </summary>
        public bool Enabled { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }

    }
}
