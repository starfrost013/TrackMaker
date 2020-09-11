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
    /// Interaction logic for SeasonManager.xaml
    /// </summary>
    public partial class SeasonManager : Window
    {
        public MainWindow MnWindow { get; set; }
        public SeasonManager()
        {
            InitializeComponent();
            MnWindow = (MainWindow)Application.Current.MainWindow; 
        }

        private void Setup()
        {
            if (MnWindow.CurrentBasin.Storms.Count == 0)
            {
                MessageBox.Show("Please add a storm to use this functionality.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Close(); 
            }

            StormList.DataContext = MnWindow.CurrentBasin.Storms; 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Setup();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // The user wants to delete a storm.
            if (StormList.SelectedIndex == -1)
            {
                // -1 means no option selected, so alert the user and then do nothing
                MessageBox.Show("No storm selected to delete. [Error Code: ESE1]", "starfrost's Track Maker", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }

            if (MnWindow.CurrentBasin.Storms[StormList.SelectedIndex].Id == MnWindow.CurrentBasin.CurrentStorm.Id)
            {
                // if we want to delete the current storm, make the currentstorm null
                MnWindow.CurrentBasin.CurrentStorm = null;
            }

            MnWindow.CurrentBasin.Storms.RemoveAt(StormList.SelectedIndex); 
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (StormList.SelectedIndex == -1)
            {
                MessageBox.Show("No storm selected to edit. [Error Code: ESE2]", "starfrost's Track Maker", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // initialise the edit ui
            EditUI Eui = new EditUI(MnWindow.CurrentBasin.Storms[StormList.SelectedIndex]);
            Eui.Owner = this;
            Eui.Show();
            StormList.DataContext = MnWindow.CurrentBasin.Storms;
            StormList.UpdateLayout();
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (StormList.SelectedIndex == -1)
            {
                MessageBox.Show("No storm selected to edit. [Error Code: ESE3]", "starfrost's Track Maker", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            for (int i = 0; i < MnWindow.CurrentBasin.Storms.Count; i++)
            {
                Storm _ = MnWindow.CurrentBasin.Storms[i];

                if (StormList.SelectedIndex == i)
                {
                    MnWindow.CurrentBasin.CurrentStorm = _;
                    MessageBox.Show($"The storm {_.Name} is now selected.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                    return; 
                }
            }
        }
    }
}
