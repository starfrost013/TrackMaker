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

    public partial class AddNewStorm : Window
    {
        MainWindow MainWindow { get; set; }

        public AddNewStorm()
        {
            InitializeComponent();

            MainWindow = (MainWindow)Application.Current.MainWindow;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Storm storm = new Storm();
                storm.Id = MainWindow.CurrentBasin.Storms.Count; // this actually makes sense.
                storm.Name = NameBox.Text;

                Logging.Log($"Adding storm with id {storm.Id} and name {storm.Name}");

                if (storm.Name == "")
                {
                    Error.Throw("You must add a name.", "Track Maker", Error.ErrorSeverity.Warning, 2);
                    storm = null;
                    return;
                }

                // Check the date. [2020-05-13] - convert from nullable datetime to datetime?

                if (Date.SelectedDate == null)
                {
                    Error.Throw("You must add a date and time.", "Error", Error.ErrorSeverity.Warning, 1);
                    return;
                }

                storm.FormationDate = (DateTime)Date.SelectedDate; // WHY IS THIS NULLABLE I MEAN ITS HELPFUL BUT YES
                storm.FormationDate = storm.FormationDate.AddHours(Convert.ToInt32(TimeHours.Text));
                storm.FormationDate = storm.FormationDate.AddMinutes(Convert.ToInt32(TimeMinutes.Text));

                // USE ATTRIBUTES V2

                if (MainWindow.CurrentBasin.Storms.Count != 0)
                {
                    if (storm.FormationDate < MainWindow.CurrentBasin.Storms[0].FormationDate)
                    {
                        MessageBox.Show("You can't have a storm start earlier than the season!", "Error I1", MessageBoxButton.OK, MessageBoxImage.Error);
                        storm = null;
                        return;
                    }
                }

                if (TypeSelect.TypeBox.Text == "Tropical cyclone")
                {
                    storm.StormType = StormType.Tropical;
                }
                if (TypeSelect.TypeBox.Text == "Subtropical cyclone")
                {
                    storm.StormType = StormType.Subtropical;
                }
                if (TypeSelect.TypeBox.Text == "Extratropical cyclone")
                {
                    storm.StormType = StormType.Extratropical;
                }
                if (TypeSelect.TypeBox.Text == "Invest / PTC")
                {
                    storm.StormType = StormType.InvestPTC;
                }
                if (TypeSelect.TypeBox.Text == "Polar low")
                {
                    storm.StormType = StormType.PolarLow;
                }
                Logging.Log($"Set storm type");

                Logging.Log("Initializing node list...");
                storm.NodeList = new List<Node>(); // initalize the mode list
                Logging.Log("Setting current storm...");
                MainWindow.CurrentBasin.CurrentStorm = storm;
                Logging.Log("Adding storm to basin storm list...");
                MainWindow.CurrentBasin.Storms.Add(storm);
                Logging.Log("Done! Closing...");
                this.Close();
            }
            catch (FormatException)
            {
                Error.Throw("You must enter a valid date and time.", "Error", Error.ErrorSeverity.Warning, 3);
                return; 
            }
        }
    }
}
