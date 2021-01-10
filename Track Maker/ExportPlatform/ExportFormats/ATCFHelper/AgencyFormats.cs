using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    /// <summary>
    /// Formats used by actual weather agencies.
    /// </summary>
    public enum AgencyFormats
    {
        /// <summary>
        /// Automated Tropical Cyclone Forecasting System format.
        /// </summary>
        ATCF = 0,

        /// <summary>
        /// Hurricane Database 2.0 format
        /// </summary>
        HURDAT2 = 1
    }
}
