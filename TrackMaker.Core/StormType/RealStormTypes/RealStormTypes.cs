using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    /// <summary>
    /// Storm types that actually exist.
    /// 
    /// ATCF/HURDAT2 ONLY 2020-12-19
    /// </summary>
    public enum RealStormType
    {
        /// <summary>
        /// Tropical cyclone
        /// </summary>
        Tropical = 0,

        /// <summary>
        /// Subtropical cyclone
        /// </summary>
        Subtropical = 1,

        /// <summary>
        /// Extratropical cyclone
        /// </summary>
        Extratropical = 2,

        /// <summary>
        /// Low (HURDAT2 Only) (2020-12-27 for Beta4)
        /// </summary>
        Invest = 3
    }
}
