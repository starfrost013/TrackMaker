using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace Track_Maker
{
    /// <summary>
    /// 
    /// MainWindow.xaml.cs
    /// 
    /// Created: 2019-11-07 (Start of Development)
    /// 
    /// Edited: 2020-05-22 ("Dano" 2.0.381.0)
    /// 
    /// Purpose: Interaction logic for MainWindow.xaml
    /// 
    /// </summary>

    public partial class MainWindow : Window
    {
        public DispatcherTimer TickTimer { get; set; }
        public CategoryManager Catman { get; set; }
        public Basin CurrentBasin { get; set; }
        public List<Basin> BasinList { get; set; }
        public static int Debug { get; set; }  // static for debug purposes
        public string LOGFILE { get; set; } // the log file path (visiblenames moved to settings for v0.3, build 168)
        public bool Fullscreen { get; set; } // v0.2, build 148 and later. V0.9: MOVE TO SETTINGS
        /// <summary>
        /// New for Priscilla.
        /// </summary>
        public Project CurrentProject { get; set; } // not the best place to put this tbh.
        public MainWindow()
        {
            Init();
        }

        public void Init()
        {
            Debug = 1;
            

            string CurrentDateTime = DateTime.Now.ToString();
            CurrentDateTime = CurrentDateTime.Replace("/", "-"); // replace backslashes with dashes as they are interpreted as subdirectories and it breaks
            LOGFILE = $"{AppDomain.CurrentDomain.BaseDirectory}{CurrentDateTime}-log.txt";
            LOGFILE = LOGFILE.Replace(":", "-");
            Logging.Log("Welcome to the Debug Collective");
            Logging.Log("-------------------------------");
            Logging.Log("© 2019-20 Cosmo. Now Loading...");
            Logging.Log("Starting phase 1..."); // log starting.
            Catman = new CategoryManager();
            Catman.InitCategories();
            Logging.Log("Initialised category manager.");
            CurrentBasin = new Basin();
            Logging.Log("Initialized current basin.");
            BasinList = new List<Basin>(); // create the list
            Logging.Log("Initialized basin list. Loading basins...");
            LoadBasins();

            // Load Settings
            Logging.Log("Loading settings...");
            LoadSettings2();

            Logging.Log("Checking for updates...");
            //Process.Start("Updater.exe");
            Init_DetermineTelemetryConsentStatus();

            Init_Phase2();
        }


        public void RunUpdater()
        {
            Process.Start("Updater.exe");
        }

        public void Init_Phase2()
        {
            // Phase 2 Init
            InitializeComponent();

            Logging.Log("Initialized window, starting phase 2...");
            TickTimer = new DispatcherTimer();
            TickTimer.Tick += TimerTicked;
            TickTimer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            TickTimer.IsEnabled = true;
            Logging.Log("Initialized global update timer...");
            Logging.Log($"Setting current basin to {CurrentBasin.Name}...");
            CurrentBasin.BasinImage = new BitmapImage();
            Logging.Log("Loading basin image...");
            CurrentBasin.BasinImage.BeginInit();
            CurrentBasin.BasinImage.UriSource = new Uri(CurrentBasin.BasinImagePath, UriKind.RelativeOrAbsolute); // hopefully valid...hopefully.
            CurrentBasin.BasinImage.EndInit();
            Logging.Log("Loaded basin image. Setting data context...");
            HurricaneBasin.DataContext = CurrentBasin;
            Logging.Log($"Starting global update timer...interval: {TickTimer.Interval}");
            UpdateLayout();
#if DANO
            Title = "Track Maker Dano (version 3.0; pre-release (Alpha 2/M2) - do not use for production purposes!)";
#elif PRISCILLA
#if DEBUG
            Title = "Track Maker \"Priscilla\" (version 2.0 alpha) (Debug Build)";
#endif
            Title = "Track Maker \"Priscilla\" (version 2.0 alpha)";
#endif

            // DisableUI test 
            if (CurrentProject == null)
            {
                FileMenu.IsEnabled = false;
                ProjectMenu.IsEnabled = true;
                EditMenu.IsEnabled = false;
                StormMenu.IsEnabled = false;
                ViewMenu.IsEnabled = false;
                BasinMenu.IsEnabled = false;
                ToolsMenu.IsEnabled = false;
                HelpMenu.IsEnabled = false;
            }

            TickTimer.Start();
            Logging.Log("Initialization completed.");
        }

        public void TimerTicked(object sender, EventArgs e) // when the timer ticks.
        {
            RenderContent(HurricaneBasin, Setting.DotSize); // Content Renderer 1.2 for v0.3+
            return;
        }

        // ok
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            // Handle when the user has pressed a key. 

            switch (e.Key)
            {
                // The user wants fullscreen mode, so let's do windowed borderless. It's better anyway.
                case Key.F11:
                    SetFullscreen(); 
                    return; 
                case Key.Y:
                    if (CurrentBasin.CurrentStorm == null) return;
                    if (CurrentBasin.CurrentStorm.NodeList.Count == 0) return;

                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                    {
                        // we want to redo
                        CurrentBasin.CurrentStorm.Redo();
                    }
                    return;
                case Key.Z:
                    if (CurrentBasin.CurrentStorm == null) return;
                    if (CurrentBasin.CurrentStorm.NodeList.Count == 0) return;

                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                    {
                        // we want to undo
                        CurrentBasin.CurrentStorm.Undo(); 
                    }
                    return;
            }
        }

        /// <summary>
        /// (1.5+) Get basin with name. Major refactoring is currently ongoing that will eventually lead to this being moved to its own class (GlobalState?)
        /// </summary>
        /// <returns></returns>
        public Basin GetBasinWithName(string Name)
        {
            foreach (Basin Basin in BasinList)
            {
                if (Basin.Name == Name) return Basin; 
            }

            return null; 
        }

        /// <summary>
        /// Temporary function
        /// </summary>
        public void EnableButtons()
        {
            FileMenu.IsEnabled = true;
            ProjectMenu.IsEnabled = true;
            EditMenu.IsEnabled = true;
            StormMenu.IsEnabled = true;
            ViewMenu.IsEnabled = true;
            BasinMenu.IsEnabled = true;
            ToolsMenu.IsEnabled = true;
            HelpMenu.IsEnabled = true;
        }
    }
}
