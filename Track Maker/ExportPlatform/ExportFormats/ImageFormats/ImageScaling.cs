#if DANO
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    public enum ImageScaling
    {
        /// <summary>
        /// [T3.0/Dano] Nearest Neighbour Upscaling
        /// </summary>
        ScaleNearestNeighbour = 0,

        /// [T3.0/Dano] Bilinear Scaling
        ScaleBilinear = 1,

        /// [T3.0/Dano] Fant Scaling
        ScaleFant = 2
    }
}
#endif