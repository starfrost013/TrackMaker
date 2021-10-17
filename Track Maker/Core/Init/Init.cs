﻿using Starfrost.UL5.Core;
using Starfrost.UL5.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Reflection; 
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Track_Maker
{
    /// <summary>
    /// TrackMaker Initialisation
    /// 
    /// Priscilla 542
    /// </summary>
    public partial class MainWindow
    {
        public void Init_Phase1()
        {
            // Init logging (Priscilla v484)
#if PRISCILLA
            // create method to concanetate this for us.

            GlobalState.V52_Init("Priscilla");
#else
            GlobalState.V52_Init("Dano");

#endif
            Logging.Init(); // temp
            Assembly Ass = Assembly.GetExecutingAssembly();

            FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(Ass.Location);
            Logging.Log($"TrackMaker Version {FVI.FileVersion}");
            Logging.Log($"Initialised Starfrost's Useful Library...version {VersionInformation.UL5MajorVersion}.{VersionInformation.UL5MinorVersion}.{VersionInformation.UL5RevisionVersion} (status:{VersionInformation.UL5Status})");
            // probably temporary code, will probably put in the utility DLL as this is used 6 times (v532 2020-11-14)

            Logging.Log("TrackMaker Start");
            Logging.Log("~..welcome to the debug collective..~");
            Logging.Log("@@@@@@@@@@ LOG STARTS HERE @@@@@@@@@@");
            Logging.Log("Starting phase 1 init..."); // log starting.

            Logging.Log("Initialized global update timer...");
            TickTimer = new Timer();
            TickTimer.Elapsed += TimerTicked;
            TickTimer.Interval = 0.000001; // yes

           

            Logging.Log("Initialising category manager...");
            Catman = new CategoryManager();
            Logging.Log("Loading categories...");
            Catman.InitCategories();

#if DANO
            Logging.Log("Initialising global state...");
            GlobalState.Init(); 
#endif

            Logging.Log("Initialising storm type manager...");
            ST2Manager = new StormTypeManager();
            ST2Manager.Init();

            // Load Settings
            Logging.Log("Loading settings...");
            SettingsLoader.LoadSettings2();
            Logging.Log("Loading basins...");
            GlobalStateP.LoadBasins();
            Init_SetCurrentCategorySystem();
            Init_SetAccentColour();
            Init_HandleEoS();
        }

        public void Init_Phase2()
        {
            Logging.Log("Phase 2 Initialisation now in progress. We successfully loaded settings and basins!");
            // Phase 2 Init
            CurrentProject = new Project();
            // temp dumb hack
            CurrentProject.AddBasin("Atlantic");
            //ImagePath = CurrentProject.SelectedBasin.ImagePath;

            InitializeComponent();
            // DUMB HACK 

            Logging.Log("Initialized window, starting phase 2...");


            Logging.Log($"Starting global update timer...interval: {TickTimer.Interval}");
            TickTimer.Enabled = true;
#if DANO
            Title = "Track Maker Dano (version 3.0 alpha) - do not use for production purposes!)";
#elif PRISCILLA
            Title = "Track Maker 2.0";
#endif
            // DisableUI test 
            if (CurrentProject == null) DisableButtons();

            Logging.Log("Initialising UI...");
            HurricaneBasin.DataContext = this;
            Layers.Layers.DataContext = this;
            Layers.UpdateLayout();

            Logging.Log("Obtaining user telemetry consent status and checking for updates...");
            TelemetryConsentAcquirer.Init_DetermineTelemetryConsentStatus();

            if (Setting.ShowBetaWarning)
            {
                Logging.Log("This is a pre-release build. Displaying beta warning...");
                Error.ShowBetaWarning();
            }

            Layers.AddLayer("Background");
            
            Layers.Layers.ToggleSelectedLayer(true);

            Logging.Log("Initialisation completed. Starting render timer...");

            UpdateLayout();
            SetFullscreen(); 

            TickTimer.Start();

        }

        private void Init_SetCurrentCategorySystem()
        {
            foreach (CategorySystem CatSystem in Catman.CategorySystems)
            {
                // select the current category system
                if (Setting.DefaultCategorySystem == CatSystem.Name)
                {
                    Logging.Log("Default category system");
                    Catman.CurrentCategorySystem = CatSystem;
                    break; 
                }
            }

            if (Catman.CurrentCategorySystem == null)
            {
                Error.Throw("Error!", "An invalid category system was selected; you likely modified Settings.xml manually - the current category system has been restored to defaults. If you did not, this is a bug in the Track Maker. Contact me at starfrost#9088 on Discord for beta support.", ErrorSeverity.Error, 225); 
                Catman.CurrentCategorySystem = Catman.CategorySystems[0];
            }
            else
            {
                return; 
            }
        }

        private void Init_SetAccentColour()
        {
            if (!Setting.AccentEnabled)
            {
                Setting.AccentColour1 = new Color { A = 255, R = 255, G = 255, B = 255 };
                Setting.AccentColour2 = new Color { A = 255, R = 255, G = 255, B = 255 };
            }
        }

        /// <summary>
        /// Handles Track Maker End of Support
        /// 
        /// (v2.0.2 - Oct 10, 2021)
        /// </summary>
        private void Init_HandleEoS()
        {
            // WOW WHY IS THE ERROR HANDLING SO POORLY DESIGNED

            if (!Setting.EoSNotificationAcknowledged)
            {
                if (MessageBox.Show($"Track Maker 1.0, 2.0, 2.0.1, and 2.0.2 are no longer supported as of October 10, 2021.\n\n" +
                $"No further updates will be received for these versions (development was halted for Track Maker Legacy on September 23, 2021).\n\n" +
                $"By continuing to use this software, you understand that the software that you are using is unsupported.\n\n" +
                $"No further updates will be released.", "End of Support Notification", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    EmeraldSettings.SetSetting("EoSNotificationAcknowledged", "True");
                }
                
            }
        }
    }
}
