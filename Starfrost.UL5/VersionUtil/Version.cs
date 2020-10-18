using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfrost.UL5.VersionUtilities
{
    /// <summary>
    /// Copyright © 2020 avant-gardé eyes
    /// 
    /// Emerald version class
    /// </summary>
    public class EVersion
    {
        public DateTime BuildDate { get; set; }
        public string BuildOwner { get; set; } // eg "Cosmo"
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; } // The build number
        public int Revision { get; set; }
        public string Status { get; set; } // eg "Beta"


    }
}
