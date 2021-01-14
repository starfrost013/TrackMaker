using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Core
{
    public enum WndStyle
    {
        /// <summary>
        /// Windowed
        /// </summary>
        Windowed = 0,
        
        /// <summary>
        /// Windowed, minimised at start
        /// </summary>
        MinimisedAtStart = 1,

        /// <summary>
        /// Windowed Borderless
        /// </summary>
        Borderless = 2,

        /// <summary>
        /// Fullscreen
        /// </summary>
        Fullscreen = 3
    }
}
