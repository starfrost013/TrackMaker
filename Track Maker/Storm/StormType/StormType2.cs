using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    /// <summary>
    /// StormType V2 (the ATCF format forced me to implement this against my will)
    /// 
    /// Priscilla   v475
    /// 
    /// 2020-10-05
    /// </summary>
    public class StormType2
    {
        /// <summary>
        /// The name of this storm type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The abbreviation of this storm type.
        /// </summary>
        public string Abbreviation { get; set; }
    }
}
