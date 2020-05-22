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
                // Dano: move to TNode.AddNodeIntensity() - add currentstorm to this
                Node TNode = new Node();
                TNode.Intensity = Convert.ToInt32(IntensityTextBox.Text);
                
                // enum.parse won't work here

                if (TypeSelect.TypeBox.SelectedIndex == 0)
                {
                    TNode.NodeType = StormType.Tropical;
                }
                else if (TypeSelect.TypeBox.SelectedIndex == 1)
                {
                    TNode.NodeType = StormType.Subtropical;
                }
                else if (TypeSelect.TypeBox.SelectedIndex == 2)
                {
                    TNode.NodeType = StormType.Extratropical;
                }
                else if (TypeSelect.TypeBox.SelectedIndex == 3)
                {
                    TNode.NodeType = StormType.InvestPTC;
                }
                else if (TypeSelect.TypeBox.SelectedIndex == 4)
                {
                    TNode.NodeType = StormType.PolarLow;
                }

                TNode.Position = Pos;

                foreach (Storm Storm in MnWindow.CurrentBasin.Storms)
                {
                    if (Storm == MnWindow.CurrentBasin.CurrentStorm)
                    {
                        TNode.Id = Storm.NodeList.Count; 
                        Storm.NodeList.Add(TNode);
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
                MessageBox.Show("Why would a hurricane be this powerful anyway?", "What?", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

    }
}
