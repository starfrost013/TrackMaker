using Dano.ACECalculator; // move to dano.app.
using Dano.AdvisoryGenerator;
using DanoUI;
using Starfrost.UL5.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using Track_Maker.DanoUIHost.AddTrackPointHost;

namespace Track_Maker
{
    public partial class MainWindow : Window
    {

        /// <summary>
        /// MOVE THIS CODE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasinMenu_BasinSwitch_Click(object sender, RoutedEventArgs e)
        {

            DanoBasinSwitcherHost DBSH = new DanoBasinSwitcherHost(CurrentProject.GetBasinNames());
            DBSH.Owner = this;
            DBSH.Show(); 
        }

        private void BasinMenu_Clear_Click(object sender, RoutedEventArgs e)
        {
            CurrentProject.SelectedBasin.CurrentLayer.CurrentStorm = null;
            CurrentProject.SelectedBasin.ClearBasin();
        }

        private void ViewMenu_Names_Click(object sender, RoutedEventArgs e) => Setting.DefaultVisibleTextNames = !Setting.DefaultVisibleTextNames;

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // left mouse button clicked (no zoom)
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Project CurProj = CurrentProject;

                if (CurProj != null && CurProj.SelectedBasin != null && CurProj.SelectedBasin.CurrentLayer != null)
                {
                    // if we have no storms, ask the user to create a storm instead of add a track point. 
                    if (CurProj.SelectedBasin.CurrentLayer.CurrentStorm == null)
                    {
                        AddNewStormHost Addstwindow = new AddNewStormHost();
                        Addstwindow.Owner = this;
                        Addstwindow.Show();
                        return;
                    }
                    else
                    {
                        // build 524
#if PRISCILLA
                        StormTypeManager ST2M = ST2Manager;
#else
                        StormTypeManager ST2Manager = GlobalState.GetST2Manager();
#endif
                        Storm SelectedStorm = CurProj.SelectedBasin.GetCurrentStorm();

                        int NodeCount = SelectedStorm.NodeList.Count - 1;

                        // build 547: implement season start time on window feature
                        AddTrackPointHost ATPHost = new AddTrackPointHost(ST2M.GetListOfStormTypeNames(), e.GetPosition(HurricaneBasin), SelectedStorm.GetNodeDate(NodeCount));
                        ATPHost.Owner = this;
                        ATPHost.Show(); 

                    }

                }

            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                // temporary code
                // this should be a matrix transformation but /shrug
                // not best practice

                LastRightMouseClickPos = e.GetPosition(HurricaneBasin); 

            }
        }

        private void EditMenu_Categories_Click(object sender, RoutedEventArgs e)
        {

            DanoCategoryManagerHost DCMH = new DanoCategoryManagerHost(Catman.GetCategorySystemNames(), Catman.GetCurrentSystemCategoryNames());
            DCMH.Owner = this;
            DCMH.Show();

            /* pre-build 427 
            CategoryManagerWindow Catmanwindow = new CategoryManagerWindow();
            Catmanwindow.Owner = this;
            Catmanwindow.Show(); */
        }

        private void FileMenu_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void HelpMenu_About_Click(object sender, RoutedEventArgs e)
        {

            AboutWindowHost AWH = new AboutWindowHost();
            AWH.Owner = this;
            AWH.Show();

            /* pre-build 436
            AboutWindow AbtWin = new AboutWindow();
            AbtWin.Owner = this;
            AbtWin.Show(); */
        }

        private void EditMenu_Season_Click(object sender, RoutedEventArgs e)
        {

            SeasonManagerHost SMH = new SeasonManagerHost(CurrentProject);
            SMH.Owner = this;
            SMH.Show();

            /* pre-build 474
            SeasonManager SMan = new SeasonManager();
            SMan.Owner = this;
            SMan.Show(); */
        }

        private void FileMenu_SaveImage_Click(object sender, RoutedEventArgs e)
        {
            ExportUI ExportUI = new ExportUI(FormatType.Export, new ExportImage());
            ExportUI.Owner = this;
            ExportUI.Show(); 
        }

        private void FileMenu_Import_XML2_Click(object sender, RoutedEventArgs e)
        {
            ExportUI ExportUI = new ExportUI(FormatType.Import, new XMLv2());
            ExportUI.Owner = this;
            ExportUI.Show(); 
        }

        private void FileMenu_Export_XML2_Click(object sender, RoutedEventArgs e)
        {
            ExportUI ExportUI = new ExportUI(FormatType.Export, new XMLv2());
            ExportUI.Owner = this;
            ExportUI.Show();
        }

        private void ToolsMenu_ACECalculator_Click(object sender, RoutedEventArgs e)
        {
            CalcMainWindow Cmain = new CalcMainWindow();
            Cmain.Owner = this;
            Cmain.Show();
        }

        private void ToolsMenu_AdvisoryGenerator_Click(object sender, RoutedEventArgs e)
        {
            AdvMainWindow Amain = new AdvMainWindow();
            Amain.Owner = this;
            Amain.Show(); 
        }

        private void HelpMenu_Preferences_Click(object sender, RoutedEventArgs e)
        {
            Settings Settings = new Settings();
            Settings.Owner = this;
            Settings.Show(); 
        }

        private void StormMenu_AddNew_Click(object sender, RoutedEventArgs e)
        {
            Logging.Log("Creating new storm window...");

            AddNewStormHost ANSH = new AddNewStormHost();
            ANSH.Owner = this;
            ANSH.Show(); 
        }

        private void FileMenu_Export_ET_Click(object sender, RoutedEventArgs e)
        {
            List<Storm> StormList = CurrentProject.SelectedBasin.GetFlatListOfStorms();

            if (StormList.Count == 0)
            {
                MessageBox.Show("You must have at least one storm to export to this format.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }

            ExportUI ExUI = new ExportUI(FormatType.Export, new ExportEasyTimeline());
            ExUI.Owner = this;
            ExUI.Show(); 
        }

        private void FileMenu_Export_BT_Click(object sender, RoutedEventArgs e)
        {
            List<Storm> StormList = CurrentProject.SelectedBasin.GetFlatListOfStorms();

            if (StormList.Count == 0)
            {
                MessageBox.Show("You must have at least one storm to export to this format.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (CurrentProject.SelectedBasin.CoordsLower != null || CurrentProject.SelectedBasin.CoordsHigher != null)
            {
                ExportUI ExUI = new ExportUI(FormatType.Export, new ExportBestTrack());
                ExUI.Owner = this;
                ExUI.Show();
            }
            else
            {
                Error.Throw("Error", "This export format is not supported by this basin. Please define coords!", ErrorSeverity.Warning, 123);
                return; 
            }

        }

        //Test code. remove this.
#if DANO
        private void DanoTest_StartPage_Click(object sender, RoutedEventArgs e)
        {
            StartPageHost SPH = new StartPageHost();
            SPH.Owner = this;
            SPH.Show();
        }


        private void DanoTest_CreateProject_Click(object sender, RoutedEventArgs e)
        {
            CreateProjectHost CPH = new CreateProjectHost(VERYTEMPORARY_DONOTUSE_AFTER_M2_PROJECT_FUNCTIONAL());
            CPH.Owner = this;
            CPH.Show();
        }
        
        private void DanoTest_PortTest_Click(object sender, RoutedEventArgs e)
        {
            DanoBasinSwitcherHost DBSH = new DanoBasinSwitcherHost(VERYTEMPORARY_DONOTUSE_AFTER_M2_PROJECT_FUNCTIONAL());
            DBSH.Owner = this;
            DBSH.Show(); 
        }
#endif

        private void ProjectMenu_New_Click(object sender, RoutedEventArgs e)
        {
            CreateProjectHost CPH = new CreateProjectHost(CurrentProject.GetBasinNames());
            CPH.Owner = this;
            CPH.Show();

            // Terrible, but temporary. 

            if (CurrentProject != null)
            {
                Layers.AddLayer("Background");
            }
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProject.SelectedBasin != null)
            {
                if (CurrentProject.SelectedBasin.CurrentLayer.CurrentStorm != null)
                {
                    CurrentProject.SelectedBasin.CurrentLayer.CurrentStorm.Undo();
                }
            }
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProject.SelectedBasin != null)
            {
                if (CurrentProject.SelectedBasin.CurrentLayer.CurrentStorm != null)
                {
                    CurrentProject.SelectedBasin.CurrentLayer.CurrentStorm.Redo();
                }
            }

        }


        private void FileMenu_Export_HURDAT_Click(object sender, RoutedEventArgs e)
        {
            ExportUI ExUI = new ExportUI(FormatType.Export, new ExportHURDAT2());
            ExUI.Owner = this;
            ExUI.Show(); 
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


        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                // Set up a translation group.
                TransformGroup TG = new TransformGroup();

                Point CurPos = e.GetPosition(HurricaneBasin);

                // Build a relative X.
                double RelativeX = CurPos.X / Width;
                double RelativeY = CurPos.Y / Height;

                // Store the current distance from the last mouse click. This allows smooth panning.
                double MouseDistanceX = CurPos.X - LastRightMouseClickPos.X;
                double MouseDistanceY = CurPos.Y - LastRightMouseClickPos.Y;

                // Create a scale transform for actually moving the "camera"
                ScaleTransform ScaleT = new ScaleTransform(ZoomLevelX, ZoomLevelY, (Width * RelativeX) + MouseDistanceX, (Height * RelativeY) + MouseDistanceY);

                // Translate the "camera" view 
                TranslateTransform TranslateT = new TranslateTransform(MouseDistanceX, MouseDistanceY);

                // Build the translation group
                TG.Children.Add(ScaleT);
                TG.Children.Add(TranslateT);
                
                // Apply the tanslations
                HurricaneBasin.RenderTransform = TG;
            }
            else
            {
                return; 
            }
        }
    }
}
