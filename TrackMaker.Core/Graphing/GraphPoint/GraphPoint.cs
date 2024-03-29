﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TrackMaker.Core.Graphing
{

    /// <summary>
    /// 3.0 (Dano) graph points
    /// </summary>
    public class GraphPoint
    {
        /// <summary>
        /// The value of this graph point.
        /// </summary>
        public object Value { get; set; } 
        public GraphPointSettings Settings { get; set; }
    }
}
