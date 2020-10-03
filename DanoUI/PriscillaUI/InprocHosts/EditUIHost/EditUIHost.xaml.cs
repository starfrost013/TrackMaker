using DanoUI;
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

namespace DanoUI.Hosts
{
    /// <summary>
    /// Interaction logic for EditUIHost.xaml
    /// </summary>
    public partial class EditUIHost : Window
    {
        public string StoName { get; set; }
        public EditUIHost(string StormName)
        {
            InitializeComponent();
            StoName = StormName; 
        }

        public void Done_Hit(object sender, DanoEventArgs DEA)
        {

        }
    }
}
