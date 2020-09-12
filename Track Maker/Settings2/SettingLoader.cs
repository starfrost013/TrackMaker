﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;

namespace Track_Maker
{
    partial class MainWindow
    {
        public void LoadSettings2()
        {
            // Loads settings using the Emerald XML Settings API.

            /* Priscilla 441
            string CBasin = EmeraldSettings.GetString("SelectedBasin");
            
            foreach (Basin basin in CurrentProject.Basins)
            {
                if (CBasin == basin.Name)
                {
                    CurrentProject.SelectedBasin = basin; 
                }
            } 

            if (CurrentProject.SelectedBasin == null)
            {
                MessageBox.Show("Invalid category system selected. Change the settings in Settings.xml or the Settings menu. The category system has been changed to the first category system installed - usually the SSHWS. If this continues to happen, contact me at starfrost#9088 on Discord.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                CurrentProject.SelectedBasin = CurrentProject.Basins[0];
            } */

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
                Setting.LineSize = 2; // reinitialies
            }

            // Do we use the gradient? 
            Setting.UseGradient = EmeraldSettings.GetBool("UseGradient"); 
            // Load the accent colours
            Setting.AccentColour1 = EmeraldSettings.GetColour("AccentColour1");
            Setting.AccentColour2 = EmeraldSettings.GetColour("AccentColour2");
            // Load experimental mode
            Setting.EnableExperimentalMode = EmeraldSettings.GetBool("ExperimentalMode");
            // Telemetry consent
            Setting.TelemetryConsent = EmeraldSettings.GetTelemetryConsent("TelemetryConsent"); 
            // Undo depth
            Setting.UndoDepth = EmeraldSettings.GetInt("UndoDepth");
            
        }
    }
}
