using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    /// <summary>
    /// Preset storm shapes.
    /// </summary>
    public enum StormShape
    {

        /// <summary>
        /// A custom shape is used - this will be used as the default value.
        /// </summary>
        Custom = 0,

        /// <summary>
        /// A circle.
        /// </summary>
        Circle = 1,

        /// <summary>
        /// A square.
        /// </summary>
        Square = 2,

        /// <summary>
        /// A triangle.
        /// </summary>
        Triangle = 3
    }
}
