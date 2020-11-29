using Starfrost.UL5.Core;
using Starfrost.UL5.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Reflection; 
using System.Timers;
using System.Threading.Tasks;

namespace Track_Maker
{
    /// <summary>
    /// TrackMaker Initialisation
    /// 
    /// Priscilla 542
    /// </summary>
    public partial class MainWindow
    {
        public void Init()
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

            Logging.Log("Initialising category manager.");
            Catman = new CategoryManager();
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

            GlobalStateP.LoadBasins(); 

            Logging.Log("Checking for updates...");

            TelemetryConsentAcquirer.Init_DetermineTelemetryConsentStatus();

            Init_Phase2();
        }

        public void Init_Phase2()
        {
            // Phase 2 Init
            CurrentProject = new Project();
            // temp dumb hack
            CurrentProject.AddBasin("Atlantic");
            //ImagePath = CurrentProject.SelectedBasin.ImagePath;

            InitializeComponent();
            // DUMB HACK 

            Logging.Log("Initialized window, starting phase 2...");
            TickTimer = new Timer();
            TickTimer.Elapsed += TimerTicked;
            TickTimer.Interval = 0.000001; // yes
            TickTimer.Enabled = true;
            Logging.Log("Initialized global update timer...");

            Logging.Log($"Starting global update timer...interval: {TickTimer.Interval}");

#if DANO
            Title = "Track Maker Dano (version 3.0; pre-release (Alpha 2/M2) - do not use for production purposes!)";
#elif PRISCILLA
            Title = "Track Maker 2.0 (Beta Release - not for production use!)";
#endif
            // DisableUI test 
            if (CurrentProject == null)
            {
                DisableButtons();
            }

            // V2
            Logging.Log("Initialising project...");
            HurricaneBasin.DataContext = this;
            //HurricaneBasinImage.DataContext = this; 

            Layers.Layers.LayerNames = CurrentProject.SelectedBasin.GetLayerNames();
            Layers.Layers.DataContext = this;
            Layers.UpdateLayout();

            UpdateLayout();
            TickTimer.Start();
            Logging.Log("Initialisation completed.");
        }

    }
}
