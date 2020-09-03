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

            DanoBasinSwitcherHost DBSH = new DanoBasinSwitcherHost(VERYTEMPORARY_DONOTUSE_AFTER_M2_PROJECT_FUNCTIONAL());
            DBSH.Owner = this;
            DBSH.Show(); 

            /* pre-build 426
            BasinSwitcher BasinSwitch = new BasinSwitcher();
            BasinSwitch.Owner = this;
            BasinSwitch.Show();
            */
        }

        private void BasinMenu_Clear_Click(object sender, RoutedEventArgs e)
        {
            CurrentBasin.CurrentStorm = null; 
            CurrentBasin.Storms.Clear();
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

            // if we have no storms, ask the user to create a storm instead of add a track point. 
            if (CurrentBasin.CurrentStorm == null)
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

        private void EditMenu_Categories_Click(object sender, RoutedEventArgs e)
        {

            DanoCategoryManagerHost DCMH = new DanoCategoryManagerHost(Catman.GetCategoryNames());
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
            AboutWindow AbtWin = new AboutWindow();
            AbtWin.Owner = this;
            AbtWin.Show();
        }

        private void EditMenu_Season_Click(object sender, RoutedEventArgs e)
        {
            SeasonManager SMan = new SeasonManager();
            SMan.Owner = this;
            SMan.Show();
        }

        private void FileMenu_SaveImage_Click(object sender, RoutedEventArgs e)
        {
            ExportUI ExportUI = new ExportUI(FormatType.Export, new ExportImage());
            ExportUI.Owner = this;
            ExportUI.Show(); 
        }

        private void FileMenu_LoadXML_Click(object sender, RoutedEventArgs e)
        {
            ExportUI ExportUI = new ExportUI(FormatType.Import, new ExportXML());
            ExportUI.Owner = this;
            ExportUI.Show(); 
        }

        private void FileMenu_SaveXML_Click(object sender, RoutedEventArgs e)
        {
            ExportUI ExportUI = new ExportUI(FormatType.Export, new ExportXML());
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

            if (CurrentBasin.Storms.Count == 0)
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
            if (CurrentBasin.Storms.Count == 0)
            {
                MessageBox.Show("You must have at least one storm to export to this format.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ExportUI ExUI = new ExportUI(FormatType.Export, new ExportBestTrack());
            ExUI.Owner = this;
            ExUI.Show(); 
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
        /// <summary>
        /// VERY TEMP! VERY TEMP! VERY TEMP!  Creates a list of strings from the names of the storms in the current basin. Move to Project.GetBasinNames() in M2/Priscilla.
        /// </summary>
        public List<string> VERYTEMPORARY_DONOTUSE_AFTER_M2_PROJECT_FUNCTIONAL()
        {
            // Create a new string list.
            List<string> _ = new List<string>();

            // Iterate through all of the storms
            foreach (Basin CurBasin in BasinList)
            {
                _.Add(CurBasin.Name);
            }

            return _;
        }



        private void ProjectMenu_New_Click(object sender, RoutedEventArgs e)
        {
            CreateProjectHost CPH = new CreateProjectHost(VERYTEMPORARY_DONOTUSE_AFTER_M2_PROJECT_FUNCTIONAL());
            CPH.Owner = this;
            CPH.Show();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentBasin != null)
            {
                if (CurrentBasin.CurrentStorm != null)
                {
                    CurrentBasin.CurrentStorm.Undo();
                }
            }
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentBasin != null)
            {
                if (CurrentBasin.CurrentStorm != null)
                {
                    CurrentBasin.CurrentStorm.Redo();
                }
            }

        }

    }
}
