using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker
{
    /// <summary>
    /// Valid image formats for image exporting
    /// </summary>
    public enum ImageFormats
    { 
        /// <summary>
        /// Portable Network Graphics
        /// Medium size, medium to high quality
        /// </summary>
        PNG = 0,

        /// <summary>
        /// JPEG 
        /// 
        /// Low size, low to medium quality
        /// </summary>
        JPEG = 1,

        /// <summary>
        /// Bitmap
        /// 
        /// High quality, very high size
        /// </summary>
        BMP = 2,

        /// <summary>
        /// TIFF
        /// </summary>
        TIFF = 3,

        /// <summary>
        /// Windows Media Picture
        /// </summary>
        WMP = 4,

        /// <summary>
        /// GIF89
        /// 
        /// Low size, very low quality, animated
        /// </summary>
        GIF = 5,

        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 6

    }
}
