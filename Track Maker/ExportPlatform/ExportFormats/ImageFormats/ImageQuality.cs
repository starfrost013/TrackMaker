using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    /// <summary>
    /// Image quality
    /// </summary>
    public enum ImageQuality
    {
        /// <summary>
        /// [T2.0] Full Quality
        /// </summary>
        Full = 0,

        /// <summary>
        /// [T2.0] Half Quality
        /// </summary>
        Half = 1,

        /// <summary>
        /// [T2.0] Quarter Quality
        /// </summary>
        Quarter = 2,

        /// <summary>
        /// [T2.0] Eighth Quality
        /// </summary>
#if !DANO
        Eighth = 3

#else
        Eighth = 3
        

        /// <summary>
        /// [T3.0/Dano] Custom
        /// </summary>
        Custom = 4,
        /// <summary>
        /// [T3.0/Dano] Nearest Neighbour Upscaling
        /// </summary>
        UpscaleNearestNeighbour = 5,

        /// [T3.0/Dano] Bilinear Scaling
        UpscaleBilinear = 6,

        /// [T3.0/Dano] Fant Scaling
        Fant = 7
#endif
    }
}
