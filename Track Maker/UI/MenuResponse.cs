using Dano.ACECalculator; // move to dano.app.
using Dano.AdvisoryGenerator;
using DanoUI; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            CurrentProject.SelectedBasin.ClearBasin();
        }

        private void ViewMenu_Names_Click(object sender, RoutedEventArgs e)
        {
            // Toggle name visibility. 
            switch (ViewMenu_Names.IsChecked)
            {
                case false:
                    Setting.DefaultVisibleTextNames = false;
                    return;
                case true:
                    Setting.DefaultVisibleTextNames = true;
                    return;
            }
           
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            if (CurrentProject != null && CurrentProject.SelectedBasin != null && CurrentProject.SelectedBasin.CurrentLayer != null)
            {
                // if we have no storms, ask the user to create a storm instead of add a track point. 
                if (CurrentProject.SelectedBasin.CurrentLayer.CurrentStorm == null)
                {
                    AddNewStorm Addstwindow = new AddNewStorm();
                    Addstwindow.Owner = this;
                    Addstwindow.Show();
                    return;
                }

                AddTrackPoint Addtrwindow = new AddTrackPoint(e.GetPosition(HurricaneBasin));
                Addtrwindow.Owner = this;
                Addtrwindow.Show();
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
            AddNewStorm AddNewStorm = new AddNewStorm();
            AddNewStorm.Show();
        }

        private void FileMenu_Export_ET_Click(object sender, RoutedEventArgs e)
        {

            if (CurrentProject.SelectedBasin.GetFlatListOfStorms().Count == 0)
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
            if (CurrentProject.SelectedBasin.GetFlatListOfStorms().Count == 0)
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

        private void FileMenu_Import_XML_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Warning!\nThis option is only for importing OLD projects created with Track Maker 1.x. It is NOT for projects created in version 2.x!\n\nLayers and multiple basins are NOT SUPPORTED in version 1.x format!\nFurthermore, this option will NOT be available in future versions of the Track Maker! You can remove the export to version 1.x option in the Settings!", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ExportUI ExUI = new ExportUI(FormatType.Import, new ExportXML());
                ExUI.Owner = this;
                ExUI.Show();
            }
        }


        private void FileMenu_Export_HURDAT_Click(object sender, RoutedEventArgs e)
        {
            ExportUI ExUI = new ExportUI(FormatType.Export, new ExportHURDAT2());
            ExUI.Owner = this;
            ExUI.Show(); 
        }

    }
}
