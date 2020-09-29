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
                // Build the date and time.

                // V2.0.471.0: Fix a crash bug involving not entering a valid date
                if (Date.SelectedDate == null)
                {
                    Error.Throw("Warning", "Please enter a valid date!", ErrorSeverity.Warning 128);
                    return; 
                }

                DateTime TD = (DateTime)Date.SelectedDate;

                int Hours = Convert.ToInt32(TimeHours.Text);
                int Minutes = Convert.ToInt32(TimeMinutes.Text);

                if (Hours < 0 || Hours > 24 || Minutes < 0 || Minutes > 59)
                {
                    Error.Throw("Warning!", "Please enter a valid time!", ErrorSeverity.Warning, 101);
                    return;
                }

                TD = TD.AddHours(Hours);
                TD = TD.AddMinutes(Minutes);

                // Kinda temp because this won't be in mainwindow after a while
                MainWindow.CurrentProject.SelectedBasin.AddStorm(NameBox.Text, TypeTextBlock.Text, TD);

                Close(); 
                return; 
            }
            catch (FormatException)
            {
                Error.Throw("Warning!", "Please enter a valid time!", ErrorSeverity.Warning, 101);
                return;
            }
            
        }
    }
}
