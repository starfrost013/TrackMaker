using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiralen.UpdateServices.ApplicationSettings
{
    /// <summary>
    /// Update Channels
    /// 
    /// An 'update channel' is a configuration file that is utilised to determine the updates that a user is entitled to.
    /// 
    /// As of 'Priscilla' 2.0.462, Update Services 2.0 has multiple channels:
    /// 
    /// 
    /// </summary>
    public enum UpdateChannel // Spec 1.0 - 2.0.462 -
    {
        /// <summary>
        /// User gets all Track Maker releases. 
        /// </summary>
        Release = 0,

        /// <summary>
        /// Beta Channel
        /// User gets beta versions of a Track Maker release. e.g. 0.93.301 Beta 23
        /// </summary>
        Beta = 1,
        
        /// <summary>
        /// Nightly (current release) channel
        /// 
        /// User gets latest builds of the current release 
        /// </summary>
        Nightly = 2,

        /// <summary>
        /// Current release+1 
        /// 
        /// User gets latest builds of the current release (currently Iris - 2.1 or 2.5)
        /// </summary>
        Iris = 3,
        
        /// <summary>
        /// Current release=2
        /// 
        /// User gets latest builds of the current release+2 (currently Dano - 3.0)
        /// </summary>
        Dano = 4,
        
        /// <summary>
        /// Private Experimentation
        /// 
        /// User gets latest builds of current experimental release (Sledgehammer/3DX v4.0)
        /// </summary>
        Sledgehammer = 5
    }
}
