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
        public static WndStyle WindowStyle { get; set; } // Window style
        public static int LineSize { get; set; } // Line size
        public static TelemetryConsent TelemetryConsent { get; set; } // Telemetry consent
        public static int UndoDepth { get; set; } // Amount of undos allowed
        public static bool UseGradient { get; set; } // Use the gradient
#if DANO
        public static bool Dano_DisableGlobalState { get; set; } // Dano (version 3.0) - disable global state (act like Priscilla)
        public static bool Dano_EnableOverlaySupport { get; set; } // Dano (version 3.0) - enable overlay support
        public static bool Dano_UseDeserialisation { get; set; } // Dano (version 3.0) - use XML Deserialisation
        public static bool Dano_UseGraphWindow { get; set: } // Dano (version 3.0) - use the GraphWindow
        public static bool Dano_UseTabUI { get; set; } // Dano (version 3.0) - use TabUI (MainWindowHost)
        public static bool Sledgehammer_Enable { get; set; } // Sledgehammer (version 4.0?) 3D enabled
        // more to come...
#endif

    }
}
