using Starfrost.UL5.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Starfrost.UL5.ScaleUtilities
{
    public static class ScaleUtilities
    {
        public static BitmapImage ScaleToQuality(this BitmapImage Img, ImageQuality ImgQuality)
        {

            switch (ImgQuality)
            {
                case ImageQuality.Full:
                    return Img;
                case ImageQuality.Half:
                    Img.DecodePixelWidth = Convert.ToInt32(Img.Width / 2); 
                    Img.DecodePixelHeight = Convert.ToInt32(Img.Width / 2);
                    return Img;
                case ImageQuality.Quarter:
                    Img.DecodePixelWidth = Convert.ToInt32(Img.Width / 4);
                    Img.DecodePixelHeight = Convert.ToInt32(Img.Width / 4);
                    return Img;
                case ImageQuality.Eighth:
                    Img.DecodePixelWidth = Convert.ToInt32(Img.Width / 8);
                    Img.DecodePixelHeight = Convert.ToInt32(Img.Width / 8);
                    return Img;
            }

            // pre-v3
            return null; 
        }
    }
}
