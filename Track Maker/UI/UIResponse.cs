﻿using Dano.ACECalculator; // move to dano.app.
using Dano.AdvisoryGenerator;
using DanoUI;
using Starfrost.UL5.Logging;
using Starfrost.UL5.MathUtil;
using Starfrost.UL5.WpfUtil; 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

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
            //MainWindow logic in MainWindow class (Hack till Iris Layer binding)
            Layers.ClearLayers();
            //end

            CurrentProject.SelectedBasin.ClearBasin();
            // iris: move initbasin to selectedbasin
            CurrentProject.InitBasin(CurrentProject.SelectedBasin);
        }

        private void ViewMenu_Names_Click(object sender, RoutedEventArgs e) => Setting.DefaultVisibleTextNames = !Setting.DefaultVisibleTextNames;

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // left mouse button clicked (no zoom)
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Project CurProj = CurrentProject;

                // fix retardation
                if (CurProj != null && CurProj.SelectedBasin != null && CurProj.SelectedBasin.CurrentLayer != null)
                {
                    // if we have no storms, ask the user to create a storm instead of add a track point. 
                    if (CurProj.SelectedBasin.CurrentLayer.CurrentStorm == null)
                    {
                        AddNewStormHost Addstwindow = new AddNewStormHost(CurProj.SelectedBasin.SeasonStartTime);
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
            InitExportUI<ExportImage>(FormatType.Export); 

            /* pre-build 627
            ExportUI ExportUI = new ExportUI(FormatType.Export, new ExportImage());
            ExportUI.Owner = tthis;
            ExportUI.Show(); */
        }

        private void FileMenu_Import_XML2_Click(object sender, RoutedEventArgs e)
        {
            InitExportUI<ExportXMLv2>(FormatType.Import);

            /* pre-build 629
            ExportUI ExportUI = new ExportUI(FormatType.Import, new ExportXMLv2());
            ExportUI.Owner = this;
            ExportUI.Show(); */
        }

        private void FileMenu_Export_XML2_Click(object sender, RoutedEventArgs e)
        {
            InitExportUI<ExportXMLv2>(FormatType.Export);
            
            /* pre-build 629
            ExportUI ExportUI = new ExportUI(FormatType.Export, new ExportXMLv2());
            ExportUI.Owner = this;
            ExportUI.Show(); */
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

            Basin CurrentBasin = CurrentProject.SelectedBasin;

            AddNewStormHost ANSH = new AddNewStormHost(CurrentBasin.SeasonStartTime);
            ANSH.Owner = this;
            ANSH.Show(); 
        }

        private void FileMenu_Export_ET_Click(object sender, RoutedEventArgs e)
        {
            InitExportUI<ExportEasyTimeline>(FormatType.Export);

            /* pre-build 629 
            List<Storm> StormList = CurrentProject.SelectedBasin.GetFlatListOfStorms();

            if (StormList.Count == 0)
            {
                MessageBox.Show("You must have at least one storm to export to this format.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }

            ExportUI ExUI = new ExportUI(FormatType.Export, new ExportEasyTimeline());
            ExUI.Owner = this;
            ExUI.Show(); */
        }

        private void FileMenu_Export_BT_Click(object sender, RoutedEventArgs e)
        {
            InitExportUI<ExportBestTrack>(FormatType.Export);

            /* pre-build 629
            List<Storm> StormList = CurrentProject.SelectedBasin.GetFlatListOfStorms();

            if (StormList.Count == 0)
            {
                Error.Throw("Warning", "You must have at least one storm to export to this format.", ErrorSeverity.Warning, 261);
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
            } */

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

            if (CurrentProject != null)
            {
                Layers.AddLayer("Background");
                // HACK
                Layers.Layers.ToggleSelectedLayer(true);
                // END HACK
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
            InitExportUI<ExportHURDAT2>(FormatType.Export);

            /* pre-build 629
            ExportUI ExUI = new ExportUI(FormatType.Export, new ExportHURDAT2());
            ExUI.Owner = this;
            ExUI.Show(); */
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
        /// Smoothly pan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.RightButton == MouseButtonState.Pressed)
            {

                if (ZoomLevelX < 1.05 || ZoomLevelY < 1.05) return;

                // set curpos at all times for rendering purposes
                Point CurPos = e.GetPosition(HurricaneBasin);

                // Build relative positions. 
                double RelativeX = LastRightMouseClickPos.X / Width;
                double RelativeY = LastRightMouseClickPos.Y / Height;

                // Store the current distance from the last mouse click. This allows smooth panning.

                double MouseDistanceX = CurPos.X - LastRightMouseClickPos.X;
                double MouseDistanceY = CurPos.Y - LastRightMouseClickPos.Y;


                /*
                restore this in iris with proper checks
                if (ZoomLevelX >= 2)
                {
                    MouseDistanceX = (CurPos.X - LastRightMouseClickPos.X) * 1 + (ZoomLevelX / 2.5);
                    MouseDistanceY = (CurPos.Y - LastRightMouseClickPos.Y) * 1 + (ZoomLevelY / 2.5);
                }
                */

                CurRelativePos = new Point(RelativeX, RelativeY);

                // Translate the "camera" view 

                TranslatePosition = new Point(MouseDistanceX, MouseDistanceY);

            }
            else
            {
                return; 
            }
        }

        /// <summary>
        /// Reset the mouse position on end of drag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            LastRightMouseClickPos = e.GetPosition(HurricaneBasin);
        }

        private void FileMenu_SaveCurrent_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalStateP.CurrentExportFormatName == null) GlobalStateP.CurrentExportFormatName = "Track_Maker.ExportXMLv2"; 

            Type EXType = Type.GetType(GlobalStateP.CurrentExportFormatName);

            IExportFormat IEXF = (IExportFormat)Activator.CreateInstance(EXType);

            IEXF.Export(CurrentProject);

            //refactoring probably needed

            string CurrentlyOpenFile = GlobalStateP.GetCurrentOpenFile();

            if (CurrentlyOpenFile == null || CurrentlyOpenFile == "")
            {
                Error.Throw("Error!", "Failed to set currently open file!", ErrorSeverity.Error, 231);
                return; 
            }

            Title = $"Track Maker 2.0 - [{GlobalStateP.GetCurrentOpenFile()}]";
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

            // hack for 2.0 only
            if (CurRelativePos.X == 0 || CurRelativePos.Y == 0) CurRelativePos = new Point(0.01, 0.01);

            if (ZoomLevelX < 1) ZoomLevelX = 1;
            if (ZoomLevelY < 1) ZoomLevelY = 1; 
            if (ZoomLevelX > 5) ZoomLevelX = 5;
            if (ZoomLevelY > 5) ZoomLevelY = 5;

        }


        private void FileMenu_Import_HURDAT2_Click(object sender, RoutedEventArgs e)
        {
            InitExportUI<ExportHURDAT2>(FormatType.Import);

            /* pre-build 629
            ExportUI EUI = new ExportUI(FormatType.Import, new ExportHURDAT2());
            EUI.Owner = this;
            EUI.Show(); */
        }

        private void HelpMenu_Help_Click(object sender, RoutedEventArgs e)
        {
            // build 630
            // this is disabled for now as the help file doesn't actually exist now

            // LaunchHelp()?

#if PRISCILLA
            string HelpFileName = @"Data\help_info.txt";
            
            if (File.Exists(HelpFileName))
            {
                ProcessStartInfo PS = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = $"notepad", // iris; space handling
                    Arguments = HelpFileName
                    
                };
                
                Process.Start(PS);
            }
            else
            {
                Error.Throw("Warning!", "Cannot find help.", ErrorSeverity.Warning, 361);
            }
#else // Iris 2.1/Dano 3.0
            string HelpFileName = "TrackMaker.chm";

            if (!File.Exists(HelpFileName))
            {
                Error.Throw("Error", $"Cannot find help file {HelpFileName}!", ErrorSeverity.Error, 342);
                return; 
            }
            else
            {
                // escape spaces for windows
                if (HelpFileName.Contains(' '))
                {
                    Process.Start($"hh \"{HelpFileName}\"");
                }
                else
                {
                    Process.Start($"hh {HelpFileName}");
                }
                
            }
#endif




        }

    }
}
