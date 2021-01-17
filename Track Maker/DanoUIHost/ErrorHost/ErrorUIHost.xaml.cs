using TrackMaker.UI; 
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
using TrackMaker.Core;

namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for ErrorUIHost.xaml
    /// </summary>
    public partial class ErrorUIHost : Window
    {
        public ErrorUIHost(int ErrorId, string ErrorString)
        {
            InitializeComponent();
            ErrorUI.SetErrorString(ErrorId, ErrorString); 
        }

        public void EOKHit(object sender, EventArgs e)
        {
            Close(); 
        }
    }
}
