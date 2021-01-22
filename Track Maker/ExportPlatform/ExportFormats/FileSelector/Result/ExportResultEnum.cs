using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    /// <summary>
    /// 2020-12-08  Connor Hyde (starfrost)
    /// 
    /// ApplicationSettings export result enum.
    /// </summary>
    public enum ExportResults
    {
        /// <summary>
        /// The export was successful.
        /// </summary>
        OK = 0,
        
        /// <summary>
        /// The user cancelled the export.
        /// </summary>
        Cancelled = 1,

        /// <summary>
        /// There was an error during the export.
        /// </summary>
        Error = 2
    }
}
