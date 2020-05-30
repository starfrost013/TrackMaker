using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            Logging.Log("Checking for updates...");
            Process.Start("Updater.exe");

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
            Logging.Log("Loading settings...");
            LoadSettings2();
            Init_Phase2();
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

    }
}
