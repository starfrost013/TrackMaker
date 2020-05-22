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
    /// Interaction logic for BasinSwitcher.xaml
    /// </summary>
    public partial class BasinSwitcher : Window
    {
        public MainWindow MnWindow;

        public BasinSwitcher()
        {
            InitializeComponent();
            // get the main window
            MnWindow = (MainWindow)Application.Current.MainWindow;
        }

        private void Setup()
        {
            BasinList.DataContext = MnWindow.BasinList; 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Setup();     
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (BasinList.SelectedIndex == -1)
            {
                // there is no item selected, so don't do that. 
                MessageBox.Show("No basin selected.", "Cosmo's Track Maker", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // get the basin the user wants to switch to
            for (int i = 0; i < MnWindow.BasinList.Count; i++)
            {
                Basin PotentialCandidate = MnWindow.BasinList[i];

                if (i == BasinList.SelectedIndex)
                {
                    MnWindow.CurrentBasin = PotentialCandidate;
                    MnWindow.HurricaneBasin.Background = new ImageBrush(new BitmapImage(new Uri(MnWindow.CurrentBasin.BasinImagePath, UriKind.RelativeOrAbsolute)));
                }
            }

            Close();
        }
    }
}
