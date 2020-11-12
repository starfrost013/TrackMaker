using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    /// <summary>
    /// CoordinateType Enum
    /// 
    /// The type of enum used for CoordinateType.ToString();
    /// 
    /// Version 2.0.470.20274   2020-09-30
    /// </summary>
    public enum CoordinateType
    {
        /// <summary>
        /// Track Maker data file format
        /// </summary>
        XML = 0,

        /// <summary>
        /// ATCF Coordinate Format (no decimal place shown, truncated to 1 decimal place)
        /// </summary>
        ATCF = 1,

        /// <summary>
        /// HURDAT2 Coordinate Format (plaintext)
        /// </summary>
        HURDAT = 2,
    }
}
