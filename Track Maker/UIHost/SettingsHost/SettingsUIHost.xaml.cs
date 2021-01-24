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
    /// Interaction logic for SettingsUIHost.xaml
    /// </summary>
    public partial class SettingsUIHost : Window
    {
        public SettingsUIHost()
        {
            InitializeComponent();
        }

        /// <summary>
        /// "Cancel" button event handler (Iris v689)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelHit(object sender, EventArgs e) => Close(); 
    }
}
