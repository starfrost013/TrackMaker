using DanoUI;
using Starfrost.UL5.Core;
using Starfrost.UL5.Logging; 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        
        /// <summary>
        /// Last right mouse click position for smooth panning (v567)
        /// </summary>
        public Point LastRightMouseClickPos { get; set; } 

        public string ImagePath { get => CurrentProject.SelectedBasin.ImagePath; set
            {

                CurrentProject.SelectedBasin.ImagePath = value;
                HurricaneBasin.Background = new ImageBrush(new BitmapImage(new Uri(value, UriKind.RelativeOrAbsolute))); 
            }
        }

        public MainWindow()
        {
            Init_Phase1();
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

            TransformGroup TG = new TransformGroup();

            ScaleTransform ST = new ScaleTransform(ZoomLevelX, ZoomLevelY);

            TG.Children.Add(ST);

            HurricaneBasin.RenderTransform = TG;
            // DUMB HACK END
        }

        private void Shutdown(object sender, EventArgs e)
        {
            TickTimer.Stop(); 
        }

#if PRISCILLA
        public void StartTimer() => SetTimerState(true);
        public void StopTimer() => SetTimerState(false); 

        private void SetTimerState(bool SetTimerState)
        {
            // ugly.
            if (TickTimer.Enabled == SetTimerState)
            {
                Error.Throw("Error!", "Attempted to start the render timer when running or stop the timer when stoppd!", ErrorSeverity.Error, 340);
            }
            else
            {
                // SET enabled?
                if (SetTimerState)
                {
                    TickTimer.Start();
                }
                else
                {
                    TickTimer.Stop();
                }
            }
        }
         
#elif IRIS
#endif
    }
}
