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
    /// Interaction logic for Priscilla_UError.xaml
    /// </summary>
    public partial class Priscilla_ErrorUI : UserControl
    {
        public EventHandler ErrorOKHit { get; set; }
        public string ErrorString { get; set; }
        public Priscilla_ErrorUI()
        {
            InitializeComponent();
            Error_ErrorText.DataContext = this; 
        }

        public void SetErrorString(string EString)
        {
            ErrorString = EString;
            UpdateLayout(); 
        }

        private void Error_OKButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorOKHit(sender, new EventArgs()); 
        }
    }
}
