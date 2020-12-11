using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker.MainWindowHost
{
    /// <summary>
    /// 'Iris' MainWindowState (track maker vNext = 2.1/3.0)
    /// </summary>
    public enum MainWindowState
    {
        Idle = 0,

        IrisBoot = 1,

        Rendering = 2,

        Importing = 3,

        Exporting = 4,
        
        Exiting = 5
    }
}
