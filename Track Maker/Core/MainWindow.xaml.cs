using DanoUI;
using Starfrost.UL5.Core;
using Starfrost.UL5.Logging; 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers; 
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
    /// Edited: 2020-10-20
    /// 
    /// Purpose: Interaction logic for MainWindow.xaml
    /// 
    /// </summary>

    public partial class MainWindow : Window
    {
        public Timer TickTimer { get; set; }
        public CategoryManager Catman { get; set; }

        /// <summary>
        /// New for Priscilla.
        /// </summary>
        public Project CurrentProject { get; set; } // not the best place to put this tbh.

        /// <summary>
        /// The Storm Type Manager. To be moved to GlobalState in 3.0
        /// </summary>
        public StormTypeManager ST2Manager { get; set; }
        
        /// <summary> 
        /// To be moved with MWH in V3 (and turned into a DependencyProperty for the sake of code simplicity)
        /// </summary>
        public double ZoomLevelX { get; set; }
        public double ZoomLevelY { get; set; }

        public DependencyProperty CentrePositionProperty = DependencyProperty.Register("CentrePosition", typeof(Point), typeof(MainWindow)); 

        public Point CentrePosition { get => (Point)GetValue(CentrePositionProperty); set => SetValue(CentrePositionProperty, value); }

        //VERY DUMB HACK?
        public string ImagePath { get => CurrentProject.SelectedBasin.ImagePath; set => CurrentProject.SelectedBasin.ImagePath = value; }

        //END VERY DUMB HACK
        public MainWindow()
        {
            Init();
        }

        public void Init()
        {
            // Init logging (Priscilla v484)

#if PRISCILLA
            // create method to concanetate this for us.
           
            GlobalState.V52_Init("Priscilla");

            Logging.Init(); // temp
            Logging.Log($"Initialised Starfrost's Useful Library...version {VersionInformation.UL5MajorVersion}.{VersionInformation.UL5MinorVersion}.{VersionInformation.UL5RevisionVersion} (status:{VersionInformation.UL5Status})");
#endif


            Logging.Log("..Welcome to the Debug Collective..");
            Logging.Log("-----------------------------------");
            Logging.Log("© 2019-20 starfrost. Now Loading...");
            Logging.Log("Starting phase 1..."); // log starting.

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

            Logging.Log("Checking for updates...");

            TelemetryConsentAcquirer.Init_DetermineTelemetryConsentStatus();

            Init_Phase2();
        }

        public void Init_Phase2()
        {
            // Phase 2 Init
            CurrentProject = new Project(true);
            // temp dumb hack
            CurrentProject.AddBasin("Atlantic");
            InitializeComponent();
            

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
            Title = "Track Maker \"Priscilla\" version 2.0 (Beta Release - not for production use!)";
#endif
            // DisableUI test 
            if (CurrentProject == null)
            {
                DisableButtons();
            }

            // V2
            Logging.Log("Initialising project...");
            HurricaneBasin.DataContext = this;

            Layers.Layers.LayerNames = CurrentProject.SelectedBasin.GetLayerNames();
            Layers.Layers.DataContext = this;
            Layers.UpdateLayout();

            UpdateLayout();
            TickTimer.Start();
            Logging.Log("Initialization completed.");
        }

        public void TimerTicked(object sender, EventArgs e) // when the timer ticks.
        {
            // This code runs on the UI thread. 
            this.Dispatcher.Invoke(() =>
            {
                RenderContent(HurricaneBasin, Setting.DotSize); // Content Renderer 1.2 for v0.3+
                return;
            });

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
                    if (CurrentProject.SelectedBasin.CurrentLayer.CurrentStorm == null) return;
                    if (CurrentProject.SelectedBasin.CurrentLayer.CurrentStorm.NodeList.Count == 0) return;

                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                    {
                        // we want to redo
                        CurrentProject.SelectedBasin.CurrentLayer.CurrentStorm.Redo();
                    }
                    return;
                case Key.Z:
                    if (CurrentProject.SelectedBasin.CurrentLayer.CurrentStorm == null) return;
                    if (CurrentProject.SelectedBasin.CurrentLayer.CurrentStorm.NodeList.Count == 0) return;

                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                    {
                        // we want to undo
                        CurrentProject.SelectedBasin.CurrentLayer.CurrentStorm.Undo(); 
                    }
                    return;
            }
        }


        /// <summary>
        /// Temporary function
        /// </summary>
        public void EnableButtons()
        {
            ProjectMenu.IsEnabled = true;
            EditMenu.IsEnabled = true;
            StormMenu.IsEnabled = true;
            ViewMenu.IsEnabled = true;
            BasinMenu.IsEnabled = true;
            ToolsMenu.IsEnabled = true;
        }

        public void DisableButtons()
        {
            EditMenu.IsEnabled = false;
            StormMenu.IsEnabled = false;
            ViewMenu.IsEnabled = false;
            BasinMenu.IsEnabled = false;
            ToolsMenu.IsEnabled = false;
        }

        private void FileMenu_Import_BT_Click(object sender, RoutedEventArgs e)
        {
            ExportUI ExpUI = new ExportUI(FormatType.Import, new ExportBestTrack());
            ExpUI.Owner = this;
            ExpUI.Show(); 
        }

        private void ZoomLevelChanged(object sender, DanoEventArgs e)
        {
            double ZoomLevel = (double)e.DanoParameters[0];
            ZoomLevelX = ZoomLevel / 100; // dumb hack 
            ZoomLevelY = ZoomLevel / 100;

            // DUMB HACK BEGIN
            // Temporary Code for Pre-Beta Only (HACK!!!!!!)
            HurricaneBasin.RenderTransform.SetValue(ScaleTransform.ScaleXProperty, ZoomLevelX);
            HurricaneBasin.RenderTransform.SetValue(ScaleTransform.ScaleYProperty, ZoomLevelY);
            // DUMB HACK END
        }

        private void Shutdown(object sender, EventArgs e)
        {
            TickTimer.Stop(); 
        }
    }
}
