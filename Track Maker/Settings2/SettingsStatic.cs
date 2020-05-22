using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media; 
namespace Track_Maker
{
    public class Setting
    {
        public static Color AccentColour1 { get; set; } // Accent colour 1
        public static Color AccentColour2 { get; set; } // Accent colour 2
        public static bool DefaultBasin { get; set; } // What is the default category system at startup?
        public static bool DefaultCategorySystem { get; set; } // What is the default category system at startup?
        public static bool DefaultVisibleTextNames { get; set; } // Are hurricane names visible by default at startup?
        public static Point DotSize { get; set; } // Dot size
        public static bool EnableExperimentalMode { get; set; } // Dano experimental mode
        public static int UndoDepth { get; set; } // Amount of undos allowed
        public static bool UseGradient { get; set; } // Use the gradient

    }
}
