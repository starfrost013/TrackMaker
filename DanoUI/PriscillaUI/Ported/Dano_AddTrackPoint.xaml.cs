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

namespace DanoUI
{
    /// <summary>
    /// Interaction logic for Dano_AddTrackPoint.xaml
    /// </summary>
    public partial class AddTrackPoint : UserControl
    {
        public List<string> TypeNames { get; set; }
        public Point MousePosition { get; set; }
        /// <summary>
        /// DanoParameters [OKHit: OK button hit]
        /// 
        /// Param 0     Int     Intensity of this track node
        /// Param 1     String  Type of this track node 
        /// Param 2     Point   Mouse position to add storm
        /// </summary>
        public EventHandler<DanoEventArgs> OKHit { get; set; }
        public AddTrackPoint()
        {
            InitializeComponent();

            UpdateLayout(); 
        }

        private void TypeSelect_Loaded(object sender, RoutedEventArgs e)
        {
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
                OKHit(this, DEA);
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
    }
}
