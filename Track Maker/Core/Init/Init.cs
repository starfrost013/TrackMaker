using TrackMaker.Util.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection; 
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input; 
using System.Windows.Media;
using TrackMaker.Core;
using TrackMaker.Util.StringUtilities;
using TrackMaker.Util.WpfUtil;

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
            Init_InitApplicationSettings();
            Init_InitBasins(); // to be merged with Init_InitGlobalState();
            Init_InitGlobalState();

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

        private void Init_InitTFM()
        {
            GlobalState.TFM = new TemporaryFileManager();
            GlobalState.TFM.ClearAllFiles(true);
        }
       

        private void Init_InitGlobalState()
        {
            // doesn't init globalstate rn

            // Initialise the category manager. 
            Logging.Log("Initialising category manager...");
            GlobalState.CategoryManager = new CategoryManager();
            Logging.Log("Loading categories...");
            GlobalState.CategoryManager.InitCategories();

            // Initialise the storm type manager.
            Logging.Log("Initialising storm type manager...");
            ST2Manager = new StormTypeManager();
            ST2Manager.Init();

            Logging.Log("Initialising DynaHotkey manager...");
            // Initialise the DynaHotkey manager. (MAINWINDOW ONLY)
            DHotkeyManager = new DynaHotkeyManager();

            Logging.Log("Setting current category system...");
            Init_SetCurrentCategorySystem();
            Logging.Log("Setting up DynaHotkey hotkeys from current category system...");
            Init_InitGlobalState_SetUpDynaHotkeyHotkeys();



        }

        /// <summary>
        /// Internal init function to set up DynaHotkeys hotkeys for the category systems. 
        /// </summary>
        private void Init_InitGlobalState_SetUpDynaHotkeyHotkeys()
        {
            List<Category> CategoryList = GlobalState.CategoryManager.CurrentCategorySystem.Categories;

            KeyConverter KConverter = new KeyConverter();

            foreach (Category Cat in CategoryList)
            {
                string CatName = Cat.Name;
                string KeyName = null;

                if (CatName.ContainsNumeric())
                {
                    int KeyPosition = CatName.GetFirstIndexOfNumeric();

                    // should be impossible
                    Debug.Assert(KeyPosition != -1);

                    KeyName = KeyPosition.ToString();

                    Key Key = (Key)KConverter.ConvertFromString(KeyName);

                    DHotkeyManager.AddNewHotkey(CatName, new List<Key> { Key } );
                    continue;
                }
                else
                {
                    string[] Words = Cat.Name.Split(' ');

                    switch (Words.Length)
                    {
                        case 0:
                            continue; // do nothing the category name is empty. This code shouldn't even run but just in case.
                        case 1:
                            string FirstWordOfCatName = Words[0];
                            // not a char because of the methods used.
                            string FirstChar = FirstWordOfCatName[0].ToString();

                            KeyName = FirstChar;
                            Key Key = (Key)KConverter.ConvertFromString(KeyName);

                            if (!DHotkeyManager.CheckIfKeyIsDuplicated(Key)) DHotkeyManager.AddNewHotkey(CatName, new List<Key>() { Key });

                            continue;  
                        default:
                            string HotkeyName = Cat.GetAbbreviatedCategoryName(CatName, 1, 1, 1);

                            if (HotkeyName.Length != 0)
                            {
                                KeyName = HotkeyName;
                                Key NewHotkey = (Key)KConverter.ConvertFromString(KeyName);
                                if (!DHotkeyManager.CheckIfKeyIsDuplicated(NewHotkey)) DHotkeyManager.AddNewHotkey(CatName, new List<Key>() { NewHotkey }); 
                            }
                            else
                            {
                                Error.Throw("Warning!", "Error generating abbreviation; skipping this DynaHotkey", ErrorSeverity.Warning, 414);
                                continue; 
                            }

                            continue; 
                            
                    }
                }

            } 
        }

        private void Init_InitApplicationSettings()
        {
            // Load ApplicationSettings
            Logging.Log("Loading settings...");

            SettingsSerialiser SS = new SettingsSerialiser();
            
            // Serialised Settings API 3.0
            // Validate and serialise the settings

            // move this block? temp until corefiles.xml
            try
            {
                SS.LoadApplicationSettings();
            }
            catch (FileNotFoundException err)
            {
#if DEBUG
                Error.Throw("Fatal Error", $"Cannot find settings XML schema!\n\n{err}", ErrorSeverity.FatalError, 409);
#else
                Error.Throw("Fatal Error", $"Cannot find settings XML schema!", ErrorSeverity.FatalError, 409);
#endif
            }

            if (ApplicationSettings.Iris_UseDeserialisation) Logging.Log("XML (de)serialisation enabled.");
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

            if (ApplicationSettings.ShowBetaWarning)
            {
                Logging.Log("This is a pre-release build. Displaying beta warning...");
                Error.ShowBetaWarning();
            }

        }

        private void Init_SetCurrentCategorySystem()
        {
            foreach (CategorySystem CatSystem in GlobalState.CategoryManager.CategorySystems)
            {
                // select the current category system
                if (ApplicationSettings.DefaultCategorySystem == CatSystem.Name)
                {
                    Logging.Log("Default category system");
                    GlobalState.CategoryManager.CurrentCategorySystem = CatSystem;
                    break; 
                }
            }

            if (GlobalState.CategoryManager.CurrentCategorySystem == null)
            {
                Error.Throw("Error!", "An invalid category system was selected; you likely modified Settings.xml manually - the current category system has been restored to defaults. If you did not, this is a bug in the Track Maker. Contact me at starfrost#9088 on Discord for beta support.", ErrorSeverity.Error, 225); 
                GlobalState.CategoryManager.CurrentCategorySystem = GlobalState.CategoryManager.CategorySystems[0];
            }
            else
            {
                return; 
            }
        }

        private void Init_SetAccentColour()
        {
            if (!ApplicationSettings.AccentEnabled)
            {
                ApplicationSettings.AccentColour1 = new Color { A = 255, R = 255, G = 255, B = 255 };
                ApplicationSettings.AccentColour2 = new Color { A = 255, R = 255, G = 255, B = 255 };
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
            if (!ApplicationSettings.Iris_EnableDebugUI)
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
            switch (ApplicationSettings.Iris_EnableGraphUI)
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
