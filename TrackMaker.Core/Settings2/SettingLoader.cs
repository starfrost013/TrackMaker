using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;

namespace TrackMaker.Core
{
    public static class SettingsLoader
    {

        /// <summary>
        /// Changed to static - does not require 
        /// </summary>
        public static void LoadSettings2()
        {
            // Loads settings using the Emerald XML Settings API.

            // Load the default category system
            Setting.DefaultCategorySystem = SettingsAPI.GetString("DefaultCategorySystem");

            // Load the text name visibility setting
            Setting.DefaultVisibleTextNames = SettingsAPI.GetBool("StormNamesVisible");

            // Load the dot size
            Setting.DotSize = SettingsAPI.GetPoint("DotSize"); 
            
            if (Setting.DotSize.X <= 0 || Setting.DotSize.Y <= 0)
            {
                Error.Throw("Error", "An invalid dot size was provided. The default dot size will be used.", ErrorSeverity.Error, 230); 
                //Recovery
                Setting.DotSize = new Point(8, 8); // reinitialise
            }

            Setting.LineSize = SettingsAPI.GetInt("LineSize");

            if (Setting.LineSize <= 0)
            {
                Error.Throw("Error", "An invalid line size was provided. The default line size will be used.", ErrorSeverity.Error, 231);
                //Recovery
                Setting.LineSize = 2; // reinitialise
            }

            // Load the accent colours
            Setting.AccentColour1 = SettingsAPI.GetColour("AccentColour1");
            Setting.AccentColour2 = SettingsAPI.GetColour("AccentColour2");

            // Load the accent enable setting
            Setting.AccentEnabled = SettingsAPI.GetBool("AccentEnabled");

            // Telemetry consent
            Setting.TelemetryConsent = SettingsAPI.GetTelemetryConsent("TelemetryConsent");

            // Show the beta warning
            Setting.ShowBetaWarning = SettingsAPI.GetBool("ShowBetaWarning");

            // Undo depth
            Setting.UndoDepth = SettingsAPI.GetInt("UndoDepth");

            // Load the window style
            Setting.WindowStyle = SettingsAPI.GetWindowStyle("WindowStyle");

            Setting.Iris_EnableGraphUI = SettingsAPI.GetBool("Iris_EnableGraphUI");
            // V2.1
            Setting.Iris_UseDeserialisation = SettingsAPI.GetBool("Iris_UseDeserialisation"); 
        }
    }
}
