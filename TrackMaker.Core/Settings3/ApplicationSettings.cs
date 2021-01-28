using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Windows;
using System.Windows.Media;
using TrackMaker.Util.StringUtilities;

namespace TrackMaker.Core
{
    [XmlRoot("ApplicationSettings")]
    public class ApplicationSettings
    {
        /// <summary>
        /// The first accent colour.
        /// </summary>
        
        public static Color AccentColour1 { get; set; }

        /// <summary>
        /// The second accent colour
        /// </summary>
        
        public static Color AccentColour2 { get; set; }

        /// <summary>
        /// Is the accent enabled?
        /// </summary>

        [XmlElement("AccentEnabled")]
        public static bool AccentEnabled { get; set; } 

        /// <summary>
        /// Clear logs by default
        /// </summary>

        [XmlElement("ClearLogs")]
        public static bool ClearLogs { get; set; } 

        /// <summary>
        /// The default category system to use. (may be changed to a CategorySystem soon)
        /// </summary>

        [XmlElement("DefaultCategorySystem")]
        public static string DefaultCategorySystem { get; set; }

        /// <summary>
        /// Are hurricane names visible by default at startup?
        /// </summary>

        [XmlElement("DefaultTextNamesVisible")]
        public static bool DefaultVisibleTextNames { get; set; } 

        /// <summary>
        /// Dot size for preset shapes
        /// </summary>
        public static Point DotSize { get; set; }


        /// <summary>
        /// Line size 
        /// </summary>

        [XmlElement("LineSize")]
        public static int LineSize { get; set; } 

        /// <summary>
        /// Show the Beta Warning message box
        /// </summary>

        [XmlElement("ShowBetaWarning")]
        public static bool ShowBetaWarning { get; set; }

        /// <summary>
        /// User telemetry consent status (essentially glorified update checking, indirectly showing every time the Track Maker is launched if enabled)
        /// 
        /// TODO: Write IValueConverter for this!
        /// </summary>

        
        public static TelemetryConsent TelemetryConsent { get; set; }

        private static string _telemetryconsent_internal { get; set; }

        [XmlElement("TelemetryConsent")]
        internal static string TelemetryConsent_Internal {
            get
            {
                return _telemetryconsent_internal;
            }
            set 
            {
                try
                {
                    TelemetryConsent = (TelemetryConsent)Enum.Parse(typeof(TelemetryConsent), value);
                }
                catch (ArgumentException err)
                {
#if DEBUG
                    Error.Throw("Error!", $"Fatal error converting TelemetryConsent!, Settings.TelemetryConsent is invalid!\n\n{err}", ErrorSeverity.Error, 410);
#else
                    Error.Throw("Error!", $"Fatal error converting TelemetryConsent!, Settings.TelemetryConsent is invalid!", ErrorSeverity.Error, 410);
#endif
                }
            }

        }

        /// <summary>
        /// [Unused for now] Undo history level
        /// </summary>

        [XmlElement("UndoDepth")]
        public static int UndoDepth { get; set; }

        /// <summary>
        /// Iris: Enable debug UI
        /// </summary>
        [XmlElement("Iris_EnableDebugUI")]
        public static bool Iris_EnableDebugUI { get; set; }

        /// <summary>
        /// Iris: Enable GraphUI
        /// </summary>

        [XmlElement("Iris_EnableGraphUI")]
        public static bool Iris_EnableGraphUI { get; set; } 

        /// <summary>
        /// Iris: Enable XML Deserialisation
        /// </summary>

        [XmlElement("Iris_UseDeserialisation")]
        public static bool Iris_UseDeserialisation { get; set; } 

        public static WndStyle WindowStyle { get; set; }

        private static string _windowstyle_internal { get; set; }
        /// <summary>
        /// TODO: Merge WndStyle and prebuilt WPF windowstyle
        /// </summary>
        [XmlElement("WindowStyle")]
        public static string WindowStyle_Internal
        {
            get
            {
                return _windowstyle_internal;
            }

            set
            {
                _windowstyle_internal = value;

                try
                {
                    WindowStyle = (WndStyle)Enum.Parse(typeof(WndStyle), value);
                }
                catch (ArgumentException)
                {
                    Error.Throw("Error!", "Fatal Error: Attempted to convert to invalid WindowStyle", ErrorSeverity.FatalError, 408);
                }
            }
        }

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
        public static bool Sledgehammer_Drag { get; set; } // enable drag
        public static bool Collaboration_HeartbeatRequired { get; set; } // track-maker.com Heartbeat required for Sledgehammer
        public static bool Collaboration_P2PModeEnabled { get; set; } // p2p mode enabled for Sledgehammer Collaboration
        public static string Collaboration_NoHeartbeat_Username { get; set; } // username used for no-heartbeat collaboration
#endif
#endif



    }
}
