﻿using TrackMaker.Util.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Reflection; 
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Controls; 
using System.Windows.Media;
using TrackMaker.Util.WpfUtil;
using TrackMaker.Core; 

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
            // init order fucked, need to split subsystem init and subsystem data loading
            Init_InitTFM();
            // Init logging
            Logging.Init(); // temp

            Init_WriteInitLog();
            Init_InitSettings();
            Init_InitBasins(); // to be merged with Init_InitGlobalState();
            Init_InitGlobalState();
            Init_SetCurrentCategorySystem();
            Init_SetAccentColour();

        }

        public void Init_Phase2()
        {
            Logging.Log("Phase 2 Initialisation now in progress. We successfully loaded settings and basins!");
            Init_InitProject();
            Init_InitUI();
            Init_InitLayers();
            Init_InitUserTelemetry();
            Logging.Log("Initialisation completed. Starting render timer...");
            SetFullscreen(); 
            UpdateLayout();
            TickTimer.Start();

        }

        private void Init_WriteInitLog()
        {
            Assembly Ass = Assembly.GetExecutingAssembly();

            FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(Ass.Location);
            Logging.Log($"TrackMaker Version {FVI.FileVersion}");
            Logging.Log($"Initialised Starfrost's Useful Library...version {VersionInformation.UL5MajorVersion}.{VersionInformation.UL5MinorVersion}.{VersionInformation.UL5RevisionVersion} (status:{VersionInformation.UL5Status})");
            // probably temporary code, will probably put in the utility DLL as this is used 6 times (v532 2020-11-14)

            Logging.Log("TrackMaker Start");
            Logging.Log("~..welcome to the debug collective..~");
            Logging.Log("@@@@@@@@@@ LOG STARTS HERE @@@@@@@@@@");
            Logging.Log("Starting phase 1 init..."); // log starting.

        }

        private void Init_InitTFM() => GlobalState.TFM = new TemporaryFileManager();

        private void Init_InitGlobalState()
        {
            // doesn't init globalstate rn

            // Initialise the category manager. 
            Logging.Log("Initialising category manager...");
            Catman = new CategoryManager();
            Logging.Log("Loading categories...");
            Catman.InitCategories();

            // Initialise the storm type manager.
            Logging.Log("Initialising storm type manager...");
            ST2Manager = new StormTypeManager();
            ST2Manager.Init();


        }

        private void Init_InitSettings()
        {
            // Load Settings
            Logging.Log("Loading settings...");
            SettingsLoader.LoadSettings2();

            if (Setting.Iris_UseDeserialisation) Logging.Log("XML (de)serialisation enabled.");
        }

        private void Init_InitProject()
        {
            // Phase 2 Init
            CurrentProject = new Project();
            // temp dumb hack
            CurrentProject.AddBasin("Atlantic");
            //ImagePath = CurrentProject.SelectedBasin.ImagePath;
        }

        /// <summary>
        /// Initialise basins (temp)
        /// </summary>
        private void Init_InitBasins()
        {
            Logging.Log("Loading basins...");
            GlobalState.LoadBasins();
        }

        /// <summary>
        /// Temp
        /// </summary>
        private void Init_InitLayers()
        {
            Layers.AddLayer("Background");
        }

        private void Init_InitUserTelemetry()
        {

            Logging.Log("Obtaining user telemetry consent status and checking for updates...");
            TelemetryConsentAcquirer.Init_DetermineTelemetryConsentStatus();

            if (Setting.ShowBetaWarning)
            {
                Logging.Log("This is a pre-release build. Displaying beta warning...");
                Error.ShowBetaWarning();
            }

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

        private void Init_InitUI()
        {

            Logging.Log("Initialising UI...");
            InitializeComponent();

            // DUMB HACK 

            Logging.Log("Initialized window, starting phase 2...");
            TickTimer = new Timer();
            TickTimer.Elapsed += TimerTicked;
            TickTimer.Interval = 0.000001; // yes
            TickTimer.Enabled = true;
            Logging.Log("Initialized global update timer...");

            Logging.Log($"Starting global update timer...interval: {TickTimer.Interval}");

            Title = "Track Maker 2.1";
            
            // DisableUI test 
            if (CurrentProject == null) DisableButtons();

            Init_ConfigureUI();

            HurricaneBasin.DataContext = this;
            Layers.Layers.DataContext = this;
            Layers.UpdateLayout();
        }

        private void Init_ConfigureUI()
        {
            Init_ConfigureDebugUI();
            Init_ConfigureGraphUI(); 
        }

        private void Init_ConfigureDebugUI()
        {
            // it's enabled by default
            if (!Setting.Iris_EnableDebugUI)
            {
                MenuItem DebugMnu = MainMenu.FindMenuItemWithName("DebugMenu");
                MainMenu.Items.Remove(DebugMnu);
            }
            else
            {
                return; 
            }
        }

        private void Init_ConfigureGraphUI()
        {
            switch (Setting.Iris_EnableGraphUI)
            {
                case true:
                    Logging.Log("Enabling GraphUI...");
                    GraphMenu.IsEnabled = true; 
                    GraphMenu_GraphWindow.IsEnabled = true; 
                    
                    return; 
                case false:
                    Logging.Log("Disabling GraphUI..."); 
                    MenuItem GraphMnu = MainMenu.FindMenuItemWithName("GraphMenu");
                    MainMenu.Items.Remove(GraphMnu); 

                    return; 
            }
        }



        
    }
}
