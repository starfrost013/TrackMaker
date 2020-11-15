using Starfrost.UL5.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls; 

namespace Starfrost.UL5.ScaleUtilities
{
    public static class ScaleUtilities
    {
        public static Canvas ScaleToQuality(this Canvas Cvs, ImageQuality ImgQuality)
        {

            switch (ImgQuality)
            {
                case ImageQuality.Full:
                    return Cvs;
                case ImageQuality.Half:
                    Cvs.Width /= 2;
                    Cvs.Height /= 2;
                    return Cvs;
                case ImageQuality.Quarter:
                    Cvs.Width /= 4;
                    Cvs.Height /= 4;
                    return Cvs;
                case ImageQuality.Eighth:
                    Cvs.Width /= 8;
                    Cvs.Height /= 8;
                    return Cvs;
            }

            // pre-v3
            return null; 
        }
    }
}
