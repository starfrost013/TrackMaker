using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Core
{
    /// <summary>
    /// TProj2 Header class for TrackMaker.Core
    /// </summary>
    public class TProj2Header
    {
        /// <summary>
        /// The author of this file.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Major version of the TProj format.
        /// </summary>
        public static int TProjFormatVersionMajor = 2;

        /// <summary>
        /// Minor version of the TProj format.
        /// </summary>
        public static int TProjFormatVersionMinor = 5; 

        public string TimeStamp { get; set; }

        public TProj2Header()
        {
            TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:SS");
        }
    }
}
