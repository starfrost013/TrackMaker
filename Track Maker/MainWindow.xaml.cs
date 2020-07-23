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
        public MainWindow()
        {
            Init();
        }

        public void Init()
        {
            Debug = 1;
            
            // Load Settings
            Logging.Log("Loading settings...");
            LoadSettings2();

            Logging.Log("Checking for updates...");
            //Process.Start("Updater.exe");
            Init_DetermineTelemetryConsentStatus();
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

            Init_Phase2();
        }

        /// <summary>
        /// Determine the users' update check/telemetry consent status.
        /// </summary>
        public void Init_DetermineTelemetryConsentStatus()
        {
            // Ask the user.
            if (Setting.TelemetryConsent == TelemetryConsent.NotDone)
            {
                if (MessageBox.Show("The Track Maker has auto-updating functionality.\n\n" +
                    "To determine if an update is available, the Track Maker must connect to the Internet.\n\n" +
                    "As a result of this a small amount of information is sent to the update server,\n" +
                    "and it can be used to determine certain aspects of your activity - for example the date of each Track Maker start.\n" +
                    "ABSOLUTELY NO personal information is sent to the update server and NO PERSONAL INFORMATION EVER WILL BE SENT.\n\n" +
                    "This information is only a byproduct of the auto-update functionality and absolutely no information is\n " +
                    "directly sent by the Track Maker.\n\n" +
                    "Please note that if you choose not to check for updates, you will not be able to automatically update the Track Maker,\n" +
                    "and you must do so manually. Do you wish to check for updates at each start of the Track Maker?", "Check for Updates?", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    EmeraldSettings.SetSetting("TelemetryConsent", "Yes");
                }
                else
                {
                    EmeraldSettings.SetSetting("TelemetryConsent", "No"); 
                }
            }
            else
            {
                if (Setting.TelemetryConsent == TelemetryConsent.Yes)
                {
                    RunUpdater();
                }
                else
                {
                    return; 
                }
            }
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
            Title = "Track Maker Dano (version 2.0; Milestone 1 pre-release - do not use for production purposes!)";
#elif PRISCILLA
            Title = "Track Maker Priscilla (version 1.5 alpha)";
#endif
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

        private void ProjectMenu_New_Click(object sender, RoutedEventArgs e)
        {
            CreateProjectHost CPH = new CreateProjectHost();
            CPH.Owner = this;
            CPH.Show();
            
        }
    }
}
