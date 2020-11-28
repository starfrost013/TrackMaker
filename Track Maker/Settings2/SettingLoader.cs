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
            string CCatSystem = EmeraldSettings.GetString("DefaultCategorySystem");

            foreach (CategorySystem CatSystem in Catman.CategorySystems)
            {
                // select the current category system
                if (CCatSystem == CatSystem.Name)
                {
                    Catman.CurrentCategorySystem = CatSystem; 
                }
            }

            if (Catman.CurrentCategorySystem == null)
            {
                MessageBox.Show("Invalid category system selected. Change the settings in Settings.xml or the Settings menu. The category system has been changed to the first category system installed - usually the SSHWS. If this continues to happen, contact me at starfrost#9088 on Discord.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Catman.CurrentCategorySystem = Catman.CategorySystems[0]; 
            }
            

            Setting.DefaultVisibleTextNames = EmeraldSettings.GetBool("StormNamesVisible");
            Setting.DotSize = EmeraldSettings.GetPoint("DotSize"); 
            
            if (Setting.DotSize.X <= 0 || Setting.DotSize.Y <= 0)
            {
                MessageBox.Show("Error LS1 - An invalid dot size was provided. The default dot size will be used.", "starfrost's Track Maker Fatal Error LS1", MessageBoxButton.OK, MessageBoxImage.Error);
                //Recovery
                Setting.DotSize = new Point(8, 8); // reinitialies
            }

            Setting.LineSize = EmeraldSettings.GetInt("LineSize");

            if (Setting.LineSize <= 0)
            {
                MessageBox.Show("Error LS2 - An invalid line size was provided. The default line size will be used.", "starfrost's Track Maker Fatal Error LS1", MessageBoxButton.OK, MessageBoxImage.Error);
                //Recovery
                Setting.LineSize = 2; // reinitialise
            }

            // Load the accent colours
            Setting.AccentColour1 = EmeraldSettings.GetColour("AccentColour1");
            Setting.AccentColour2 = EmeraldSettings.GetColour("AccentColour2");

            // Telemetry consent
            Setting.TelemetryConsent = EmeraldSettings.GetTelemetryConsent("TelemetryConsent"); 
            // Undo depth
            Setting.UndoDepth = EmeraldSettings.GetInt("UndoDepth");

            // Do we use the gradient? 
            Setting.UseGradient = EmeraldSettings.GetBool("UseGradient");

            // Load the window style
            Setting.WindowStyle = EmeraldSettings.GetWindowStyle("WindowStyle");

            // V2.1
            Setting.Dano_UseDeserialisation = EmeraldSettings.GetBool("Dano_UseDeserialisation"); 
        }
    }
}
