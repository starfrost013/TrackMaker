using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;

namespace Track_Maker
{
    public static class SettingsLoader
    {

        /// <summary>
        /// Changed to static - does not require 
        /// </summary>
        public static void LoadSettings2()
        {
            // Loads settings using the Emerald XML Settings API.

            // Get the GlobalState
#if DANO
            CategoryManager Catman = GlobalState.GetCategoryManager(); 
#else
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
            CategoryManager Catman = MnWindow.Catman;

#endif
            // Load the default category system
            Setting.DefaultCategorySystem = EmeraldSettings.GetString("DefaultCategorySystem");

            // Load the text name visibility setting
            Setting.DefaultVisibleTextNames = EmeraldSettings.GetBool("StormNamesVisible");

            // Load the dot size
            Setting.DotSize = EmeraldSettings.GetPoint("DotSize"); 
            
            if (Setting.DotSize.X <= 0 || Setting.DotSize.Y <= 0)
            {
                Error.Throw("Error", "An invalid dot size was provided. The default dot size will be used.", ErrorSeverity.Error, 230); 
                //Recovery
                Setting.DotSize = new Point(8, 8); // reinitialise
            }

            Setting.LineSize = EmeraldSettings.GetInt("LineSize");

            if (Setting.LineSize <= 0)
            {
                Error.Throw("Error", "An invalid line size was provided. The default line size will be used.", ErrorSeverity.Error, 231);
                //Recovery
                Setting.LineSize = 2; // reinitialise
            }

            // Load the accent colours
            Setting.AccentColour1 = EmeraldSettings.GetColour("AccentColour1");
            Setting.AccentColour2 = EmeraldSettings.GetColour("AccentColour2");

            // Load the accent enable setting
            Setting.AccentEnabled = EmeraldSettings.GetBool("AccentEnabled"); 

            // Telemetry consent
            Setting.TelemetryConsent = EmeraldSettings.GetTelemetryConsent("TelemetryConsent");

            // Show the beta warning
            Setting.ShowBetaWarning = EmeraldSettings.GetBool("ShowBetaWarning");

            // Undo depth
            Setting.UndoDepth = EmeraldSettings.GetInt("UndoDepth");

            // Load the window style
            Setting.WindowStyle = EmeraldSettings.GetWindowStyle("WindowStyle");

            // V2.1
            Setting.Dano_UseDeserialisation = EmeraldSettings.GetBool("Dano_UseDeserialisation"); 
        }
    }
}
