using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.UI.VersionUtilities
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
        public int Minor { get; set; } // Build minor
        public int Build { get; set; } // The build number
        public int Revision { get; set; } // Build revision
        public Status Status { get; set; } // Release status of this build.
        public string FullStatus { get; set; } // eg "Beta"


    }
}
