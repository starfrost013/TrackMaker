using TrackMaker.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TrackMaker.UI.ScaleUtilities
{
    public static class ScaleUtilities
    {
        public static Point ScaleToQuality(BitmapImage Img, ImageQuality ImgQuality)
        {

            switch (ImgQuality)
            {
                case ImageQuality.Full:
                    return new Point(Img.PixelWidth, Img.PixelHeight);
                case ImageQuality.Half:
                    return new Point(Img.PixelWidth / 2, Img.PixelHeight / 2);
                case ImageQuality.Quarter:
                    return new Point(Img.PixelWidth / 4, Img.PixelHeight / 4);
                case ImageQuality.Eighth:
                    return new Point(Img.PixelWidth / 8, Img.PixelHeight / 8);
            }

            // pre-v3
            return new Point(0, 0); 
        }
    }
}
