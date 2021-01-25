using System;
using System.Collections.Generic;
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
using TrackMaker.Core;

namespace TrackMaker.UI
{
    /// <summary>
    /// Interaction logic for SettingsUI.xaml
    /// </summary>
    public partial class SettingsUI : UserControl
    {
        public EventHandler CancelHit { get; set; }
        public EventHandler SaveHit { get; set; }

        public SettingsUI()
        {
            InitializeComponent();
        }

        private void ApplicationSettings_Tab_Appearance_DotSizeXSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void ApplicationSettings_Tab_Appearance_DotSizeYSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void ApplicationSettings_Tab_Appearance_LineSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void ApplicationSettings_Tab_Done_Click(object sender, RoutedEventArgs e) => SaveSettings();

        /// <summary>
        /// Sets ApplicationSettings to the user's new settings. 
        /// </summary>
        private void SaveSettings()
        {
            Color AccentColour1 = ApplicationSettings_Tab_Appearance_AccentColour1Picker.SelectedColour;
            Color AccentColour2 = ApplicationSettings_Tab_Appearance_AccentColour2Picker.SelectedColour;
            
            bool AccentEnabled = (bool)ApplicationSettings_Tab_Appearance_AccentEnabledCheckBox.IsChecked;
            bool ClearLogs = (bool)ApplicationSettings_Tab_General_ClearLogs.IsChecked;
            string DefaultCategorySystem = ApplicationSettings_Tab_General_DefaultCategorySystemBox.Text;
            WndStyle DefaultWindowStyle = (WndStyle)Enum.Parse(typeof(WndStyle), ApplicationSettings_Tab_General_DefaultWindowStyleBox.Text);
            Point DotSize = new Point(Convert.ToInt32(ApplicationSettings_Tab_Appearance_DotSizeXText), Convert.ToInt32(ApplicationSettings_Tab_Appearance_DotSizeYText));
            int LineSize = Convert.ToInt32(ApplicationSettings_Tab_Appearance_LineSizeText);

            ApplicationSettings.AccentColour1 = AccentColour1;
            ApplicationSettings.AccentColour2 = AccentColour2;
            ApplicationSettings.AccentEnabled = AccentEnabled;
            ApplicationSettings.ClearLogs = ClearLogs;
            ApplicationSettings.DefaultCategorySystem = DefaultCategorySystem;

            ApplicationSettings.DotSize = DotSize;
            ApplicationSettings.LineSize = LineSize;

            ApplicationSettings.WindowStyle = DefaultWindowStyle;

            SaveHit(this, new EventArgs());


        }

        private void ApplicationSettings_Tab_Cancel_Click(object sender, RoutedEventArgs e)
        {
            // this event has no event data
            CancelHit(this, new EventArgs()); 
        }

        private void ApplicationSettings_Tab_Appearance_AccentColour1Picker_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ApplicationSettings_Tab_Appearance_AccentColour2Picker_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
