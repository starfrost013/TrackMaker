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
using System.Windows.Shapes;

namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public MainWindow MnWindow { get; set; } // MVVM would be better here
        public Settings()
        {
            InitializeComponent();
            MnWindow = (MainWindow)Application.Current.MainWindow; 
        }

        private void Settings_Init()
        {

            // Set up initial setting values - Would use a binding and dependencyproperties, but CBA - almost done

            Settings_Tab_General_DefaultBasinBox.DataContext = MnWindow.BasinList; // Change for Basins 2.0 

            Settings_Tab_General_DefaultCatSystemBox.DataContext = MnWindow.Catman.CategorySystems; // category system 

            Settings_Tab_Appearance_DotSizeXSlider.Value = Setting.DotSize.X;
            Settings_Tab_Appearance_DotSizeYSlider.Value = Setting.DotSize.Y;

            Settings_Tab_Appearance_GradientEnabledCheckBox.IsChecked = Setting.UseGradient;

            // Set up some more stuff (can we use bindings?)

            Settings_Tab_Appearance_DotSizeXText.Text = Settings_Tab_Appearance_DotSizeXSlider.Value.ToString();
            Settings_Tab_Appearance_DotSizeYText.Text = Settings_Tab_Appearance_DotSizeYSlider.Value.ToString();

            // Ugly hack, but I can't be bothered to create a dependency property when I have half-life 2 sitting on my desktop ready to be streamed again and being almost finished with this damn project that took way too long
            // for something that isn't really a big project. for emerald, sure, but not this. Move to dependencyproperty in Dano.
            Settings_Tab_Appearance_AccentColourPicker.SelectedColour = Setting.AccentColour1;
            Settings_Tab_Appearance_AccentColourPicker.UpdateRectangle();

            // Ditto
            Settings_Tab_Appearance_MenuGradientSecondColourPicker.SelectedColour = Setting.AccentColour2;
            Settings_Tab_Appearance_MenuGradientSecondColourPicker.UpdateRectangle();

        }

        private void Settings_Tab_Appearance_AccentColourPicker_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Settings_Tab_Appearance_AccentColourPicker.ShowDialog(); 
        }

        private void Settings_Tab_Appearance_MenuGradientSecondColourPicker_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Ditto
            Settings_Tab_Appearance_MenuGradientSecondColourPicker.ShowDialog();
        }

        private void Settings_Tab_General_Done_Click(object sender, RoutedEventArgs e)
        {
            Settings_Close();
        }

        private void Settings_Tab_Appearance_Done_Click(object sender, RoutedEventArgs e)
        {
            Settings_Close(); 
        }

        private void Settings_Close()
        {
            // Move these to strings so we can set the local settings easier
            string _Accent1 = $"{Settings_Tab_Appearance_AccentColourPicker.SelectedColour.R},{Settings_Tab_Appearance_AccentColourPicker.SelectedColour.G},{Settings_Tab_Appearance_AccentColourPicker.SelectedColour.B}";
            string _Accent2 = $"{Settings_Tab_Appearance_MenuGradientSecondColourPicker.SelectedColour.R},{Settings_Tab_Appearance_MenuGradientSecondColourPicker.SelectedColour.G},{Settings_Tab_Appearance_MenuGradientSecondColourPicker.SelectedColour.B}";
            string _Accent3 = Settings_Tab_Appearance_GradientEnabledCheckBox.IsChecked.ToString();
            string _DefaultBasin = Settings_Tab_General_DefaultBasinBox.Text;
            string _DefaultCatsystem = Settings_Tab_General_DefaultCatSystemBox.Text;
            string _DotSize = $"{Settings_Tab_Appearance_DotSizeXText.Text.ToString()},{Settings_Tab_Appearance_DotSizeYText.Text.ToString()}";
            
            // This is for autoupdating.
            EmeraldSettings.SetSetting(SettingFile.Game, "AccentColour1", _Accent1);
            EmeraldSettings.SetSetting(SettingFile.Game, "AccentColour2", _Accent2);
            EmeraldSettings.SetSetting(SettingFile.Game, "UseGradient", _Accent3);
            EmeraldSettings.SetSetting(SettingFile.Game, "DotSize", _DotSize);
            EmeraldSettings.SetSetting(SettingFile.Game, "SelectedBasin", _DefaultBasin);
            EmeraldSettings.SetSetting(SettingFile.Game, "DefaultCategorySystem", _DefaultCatsystem);

            Setting.DotSize = new Point(Settings_Tab_Appearance_DotSizeXSlider.Value, Settings_Tab_Appearance_DotSizeYSlider.Value);

            // Show a message box telling the user they need to restart to save their settings
            MessageBox.Show("You must restart the Track Maker for these settings to take effect.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            Close();
        }

        private void Settings_Tabber_Loaded(object sender, RoutedEventArgs e)
        {
            Settings_Init();
        }

        // DANO - MOVE TO BINDINGS! //
        private void Settings_Tab_Appearance_DotSizeXSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) //make binding
        {
            Settings_Tab_Appearance_DotSizeXText.Text = Utilities.RoundNearest(Settings_Tab_Appearance_DotSizeXSlider.Value, 1).ToString();
        }

        private void Settings_Tab_Appearance_DotSizeYSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Settings_Tab_Appearance_DotSizeYText.Text = Utilities.RoundNearest(Settings_Tab_Appearance_DotSizeYSlider.Value, 1).ToString();
        }

        // END DANO - MOVE TO BINDINGS! //
    }
}
