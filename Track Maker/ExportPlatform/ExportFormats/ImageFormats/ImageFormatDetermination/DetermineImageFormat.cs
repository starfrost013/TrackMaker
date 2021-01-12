using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging; 
namespace Track_Maker
{
    /// <summary>
    /// move to trackmaker.core when trackmaker.core is working
    /// </summary>
    public static class ImageFormatDeterminer
    {
        /// <summary>
        /// Determine the image format from a string.
        /// </summary>
        /// <param name="FileName">The file name to parse for file extensions.</param>
        /// <returns></returns>
        public static ImageFormats FromString(string FileName)
        {
            string[] FName = FileName.Split('.');

            if (FName.Length < 1)
            {
                return ImageFormats.Unknown; 
            }
            else
            {
                string FileExtension = FName[1];

                switch (FileExtension)
                {
                    // possibly the simple way was better 
                    case "PNG":
                    case "png":
                        return ImageFormats.PNG;
                    case "JPG":
                    case "jpg":
                    case "JPEG":
                    case "Jpeg":
                        return ImageFormats.JPEG;
                    case "GIF":
                    case "gif":
                        return ImageFormats.GIF;
                    case "TIFF":
                    case "tiff":
                        return ImageFormats.TIFF;
                    case "WMP":
                    case "wmp":
                        return ImageFormats.WMP;
                    case "BMP":
                    case "bmp":
                        return ImageFormats.BMP;
                    default:
                        return ImageFormats.Unknown;
                        
                }
            }



        }

        public static BitmapEncoder GetBitmapEncoder(ImageFormats ImageFormat)
        {
            switch (ImageFormat)
            {
                case ImageFormats.PNG:
                    return new PngBitmapEncoder();
                case ImageFormats.JPEG:
                    return new JpegBitmapEncoder();
                case ImageFormats.BMP:
                    return new BmpBitmapEncoder();
                case ImageFormats.TIFF:
                    return new TiffBitmapEncoder();
                case ImageFormats.WMP:
                    return new WmpBitmapEncoder();
                case ImageFormats.GIF:
                    return new GifBitmapEncoder();
                case ImageFormats.Unknown:
                    return null;
                default:
                    return null;
            }
        }
    }
}
