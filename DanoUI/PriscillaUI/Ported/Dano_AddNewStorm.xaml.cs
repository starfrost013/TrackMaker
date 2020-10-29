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
    /// Interaction logic for Priscilla_AddNewStorm.xaml
    /// </summary>
    public partial class AddNewStorm : UserControl
    {
        /// <summary>
        /// DanoEventArgs [OKHit-v507] 
        /// 
        /// DanoParameters
        /// 0   String      The name of this storm.
        /// 1   String      The storm type of this storm.
        /// 2   DateTime    The date and time of this storm.
        /// </summary>
        public EventHandler<DanoEventArgs> OKHit { get; set; }
        public AddNewStorm()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
