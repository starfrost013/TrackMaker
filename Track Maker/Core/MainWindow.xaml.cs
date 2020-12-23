using DanoUI;
using Starfrost.UL5.Core;
using Starfrost.UL5.Logging;
using Starfrost.UL5.WpfUtil;
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
    /// Edited: 2020-12-23 (Priscilla-v2release:2.0.612.20358)
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
        private double ZoomLevelX { get; set; }
        private double ZoomLevelY { get; set; }

        public DependencyProperty CentrePositionProperty = DependencyProperty.Register("CentrePosition", typeof(Point), typeof(MainWindow)); 

        public Point CentrePosition { get => (Point)GetValue(CentrePositionProperty); set => SetValue(CentrePositionProperty, value); }
        
        /// <summary>
        /// Last right mouse click position for smooth panning (v567)
        /// </summary>
        private Point LastRightMouseClickPos { get; set; } 

        public string ImagePath { get => CurrentProject.SelectedBasin.ImagePath; set
            {

                CurrentProject.SelectedBasin.ImagePath = value;
                HurricaneBasin.Background = new ImageBrush(new BitmapImage(new Uri(value, UriKind.RelativeOrAbsolute))); 
            }
        }


        /// <summary>
        /// Used for transform persistence
        /// </summary>
        public List<Transform> InternalTransformGroup { get; set; }

        public MainWindow()
        {
            Init_Phase1();
            Init_Phase2();
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
            //TODO: fix zoom reset position by storing current transforms in a list
            //in the mainwindow? or similar.
            ScaleTransform ST = new ScaleTransform(ZoomLevelX, ZoomLevelY);


            if (InternalTransformGroup.Count != 0)
            {
                TranslateTransform TT = TransformUtil<TranslateTransform>.FindTransformWithClass(InternalTransformGroup);
                if (TT != null) TG.Children.Add(TT);
            }

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
                Error.Throw("Error!", "Attempted to start the render timer when running or stop the timer when stopped!", ErrorSeverity.Error, 340);
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

        private void Shutdown(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

#elif IRIS
#endif
    }
}
