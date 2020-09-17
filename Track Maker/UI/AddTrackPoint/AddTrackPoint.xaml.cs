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
    /// This whole thing is schizo and will be rewritten in Dano - UI will be separated from code
    /// </summary>
    public partial class AddTrackPoint : Window
    {
        public MainWindow MnWindow { get; set; }
        public Point Pos { get; set; }
        public AddTrackPoint(Point Position)
        {
            InitializeComponent();
            MnWindow = (MainWindow)Application.Current.MainWindow;
            Pos = Position; // setup 
        }

        private void TypeOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Clean up later.

                foreach (Storm Storm in MnWindow.CurrentProject.SelectedBasin.CurrentLayer.AssociatedStorms)
                {
                    if (Storm == MnWindow.CurrentProject.SelectedBasin.CurrentLayer.CurrentStorm)
                    {
                        Storm.AddNode(Convert.ToInt32(IntensityTextBox.Text), TypeSelect.TypeBox.SelectedIndex, Pos); 
                    }
                }

                Close();
            }
            catch (FormatException)
            {
                // nope
                MessageBox.Show("What did you put in the Intensity box??", "What?", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            catch (OverflowException)
            {
                MessageBox.Show("Hyper Mini Black Holes are stupid.", "What?", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

    }
}
