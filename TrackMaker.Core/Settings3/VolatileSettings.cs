using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TrackMaker.Core
{
    /// <summary>
    /// ApplicationSettings that are specific to the Track Maker session.
    /// 
    /// Used to reduce MainWindow use
    /// </summary>
    public static class VolatileApplicationSettings
    {
        public static Point WindowSize { get; set; }
    }
}
