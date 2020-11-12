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

namespace Dano.AdvisoryGenerator
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ForecastManager : Window
    {
        public AdvMainWindow MnWindow { get; set; }

        public ForecastManager(AdvMainWindow MainWindow)
        {
            InitializeComponent();
            MnWindow = MainWindow; // probably shitty.

            if (MnWindow.ForecastList.Count > 0)
            {
                // ok 
                foreach (Forecast forecast in MnWindow.ForecastList)
                {
                    Forecasts.Items.Add(forecast); // woo. 
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Forecast Forecast = new Forecast { Intensity = 40, DateTime = new DateTime(2008, 8, 19, 6, 27, 50) };
                DateTime DateTime = (DateTime)MnWindow.Date.SelectedDate;
                DateTime = DateTime.AddHours(Convert.ToInt32(MnWindow.Hours.Text)); // convert it.
                DateTime = DateTime.AddMinutes(Convert.ToInt32(MnWindow.Minutes.Text));
                // REALLY SHITTY. We should be using something in MainWindow - currentdatetime, and saving the date/time of the advisory. We will do this later if people want deleting forecast points.
                DateTime = DateTime.AddHours(6 * Forecasts.Items.Count + 2);
                Forecast.DateTime = DateTime;
                Forecast.Intensity = Convert.ToInt32(IntensityBox.Text); // yeah
                Forecast.Position = $"{PositionNS.Text}{PositionNSBox.Text} {PositionWE.Text}{PositionWEBox.Text}";
                Forecasts.Items.Add(Forecast);
                MnWindow.ForecastList.Add(Forecast); // yes...
            }
            catch (FormatException)
            {
                MessageBox.Show("An invalid input was entered", "Forecast Manager", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
