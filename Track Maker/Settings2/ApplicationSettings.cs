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
        public static bool AccentEnabled { get; set; } // Accent Enabled
        public static string DefaultCategorySystem { get; set; } // What is the default category system at startup? (v559: for some ungodly reason, this was a BOOLEAN?!)
        public static bool DefaultVisibleTextNames { get; set; } // Are hurricane names visible by default at startup?
        public static Point DotSize { get; set; } // Dot size
        public static int LineSize { get; set; } // Line size
        public static TelemetryConsent TelemetryConsent { get; set; } // Telemetry consent
        public static int UndoDepth { get; set; } // Amount of undos allowed
        public static bool Dano_UseDeserialisation { get; set; } // Priscilla+ (version 2.1) - use XML Deserialisation
        public static bool UseGradient { get; set; } // Use the gradient
        public static WndStyle WindowStyle { get; set; } // Window style

        // more to come...
#if DANO
        public static bool Dano_DisableGlobalState { get; set; } // Dano (version 3.0) - disable global state (act like Priscilla)
        public static bool Dano_EnableAnimation { get; set; } // Dano (version 3.0) - enable animation mode
        public static bool Dano_EnableOverlaySupport { get; set; } // Dano (version 3.0) - enable overlay support
        public static bool Dano_ResizableUIEnabled [ get; set; } // Dano (version 3.0) - Resizable UI Enabled

        public static bool Dano_UseErrorHandlingV2 { get; set; } // Dano (version 3.0) - use Error Handling 2.0 (delegate, central registration)
        public static bool Dano_UseGraphWindow { get; set: } // Dano (version 3.0) - use the GraphWindow
        public static bool Dano_UseTabUI { get; set; } // Dano (version 3.0) - use TabUI (MainWindowHost)
#if SLEDGEHAMMER
        public static bool Sledgehammer_Enable { get; set; } // Sledgehammer (version 4.0?) 3D enabled
        public static bool Collaboration_HeartbeatRequired { get; set; } // track-maker.com Heartbeat required for Sledgehammer
        public static bool Collaboration_P2PModeEnabled { get; set; } // p2p mode enabled for Sledgehammer Collaboration
        public static string Collaboration_NoHeartbeat_Username { get; set; } // username used for no-heartbeat collaboration
#endif
#endif



    }
}
