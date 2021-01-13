using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    /// <summary>
    /// String formats used by the Track Maker.
    /// 
    /// Priscilla v2.0.591.0    2020-12-15
    /// Priscilla v2.0.630.0    2020-12-29  Split ATCF and HURDAT2
    /// 
    /// Iris: Merge this and AgencyFormats somehow
    /// 
    /// </summary>
    public enum CoordinateFormat
    {
        /// <summary>
        /// Track Maker coordinate format
        /// </summary>
        TrackMaker = 0,

        /// <summary>
        /// ATCF / HURDAT2 coordinate format
        /// </summary>
        ATCF = 1,

        /// <summary>
        /// HURDAT2 coordinate format
        /// </summary>
        HURDAT2 = 2
    }
}
