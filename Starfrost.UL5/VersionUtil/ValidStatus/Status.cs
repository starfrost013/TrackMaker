using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.UI
{
    /// <summary>
    /// Valid statuses for a UL5 app
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Release
        /// </summary>
        Release = 0,

        /// <summary>
        /// Debug
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Prerelease
        /// </summary>
        Prerelease = 2,

        /// <summary>
        /// Prerelease Debug
        /// </summary>
        PrereleaseDebug = 3,

        /// <summary>
        /// Limited Distribution
        /// </summary>
        ExternalTesting = 4,

    }
}
