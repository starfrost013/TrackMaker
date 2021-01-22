using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Core.StaticSerialiser
{
    /// <summary>
    /// 2021-01-21  20:00 (starfrost)
    /// 
    /// Enhanced from the original sample to have enhanced return code functionality, to not use winforms, and to refactor into multiple files.
    /// </summary>
    public class StaticSerialisationResult
    {
        public string Message { get; set; }

        public bool Successful { get; set; }


    }
}
