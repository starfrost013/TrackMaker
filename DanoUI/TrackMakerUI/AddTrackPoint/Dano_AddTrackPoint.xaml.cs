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

namespace TrackMaker.UI
{
    /// <summary>
    /// Interaction logic for Dano_AddTrackPoint.xaml
    /// </summary>
    public partial class AddTrackPoint : UserControl
    {
        public List<string> TypeNames { get; set; }
        public DateTime StartTime { get; set; }
        public Point MousePosition { get; set; }

        /// <summary>
        /// Event triggered when the Advanced ApplicationSettings togglebox is hit.
        /// 
        /// Param 0     Bool    The state of the Advanced ApplicationSettings togglebox.
        /// </summary>

        public EventHandler<DanoEventArgs> AdvancedSettingsToggleChanged { get; set; }

        /// <summary>
        /// DanoParameters [OKHit: OK button hit]
        /// 
        /// Param 0     Int     Intensity of this track node
        /// Param 1     String  Type of this track node 
        /// Param 2     Point   Mouse position  
        /// Param 3     String  Pressure
        /// 
        /// </summary>
        public EventHandler<DanoEventArgs> OKHit { get; set; }
        public AddTrackPoint()
        {
            InitializeComponent();

            UpdateLayout(); 
        }


        private void TypeSelect_Loaded(object sender, RoutedEventArgs e)
        {

            // wow visual studio added a tostring format helper, when did they do that? 
            SeasonStartTimeText.Text = StartTime.ToString("yyyy-MM-dd HH:mm:ss");
            TypeSelect.Setup(TypeNames); 
        }

        private void TypeOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DanoEventArgs DEA = new DanoEventArgs();
                DEA.DanoParameters.Add(Convert.ToInt32(IntensityTextBox.Text));
                DEA.DanoParameters.Add(TypeSelect.GetSelectedItem());
                DEA.DanoParameters.Add(MousePosition);

                if (OptionalApplicationSettings_PressureBox.Text == string.Empty)
                {
                    // Add a placeholder in the case the pressure has not been specified.
                    DEA.DanoParameters.Add(1000);
                }
                else
                {
                    DEA.DanoParameters.Add(Convert.ToInt32(OptionalApplicationSettings_PressureBox.Text));
                }
                

                OKHit(this, DEA);
            }
            catch (OverflowException err)
            {
#if DEBUG
                MessageBox.Show($"Please enter an intensity between -2,147,483,647mph and 2,147,483,647mph inclusive.\n\n{err}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
#else  
                MessageBox.Show($"Please enter an intensity between -2,147,483,647mph and 2,147,483,647mph inclusive.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
            }
            catch (FormatException err) 
            {
                // For (pre-)beta only
#if DEBUG
                MessageBox.Show($"Please enter a valid intensity!\n\n{err}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
#else  
                MessageBox.Show($"Please enter a valid intensity!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
            }
        }

        private void ShowOptionalApplicationSettings_Checked(object sender, RoutedEventArgs e)
        {
            bool AdvancedApplicationSettingsChecked = (bool)ShowOptionalApplicationSettings.IsChecked; 

            DanoEventArgs DEA = new DanoEventArgs();
            DEA.DanoParameters.Add(AdvancedApplicationSettingsChecked);
            AdvancedSettingsToggleChanged(this, DEA);
        }
    }
}
