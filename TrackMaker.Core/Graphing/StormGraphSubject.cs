using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Core.Graphing
{
    /// <summary>
    /// Subjects used for storm graphing. (UI side only!)
    /// </summary>
    public enum StormGraphSubject
    {
        Intensity = 0,

        Pressure = 1,

#if DANO
        
        Category = 2,

        Size = 3,

        WindQuadrants = 4
        
#endif

    }
}
