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
    /// Interaction logic for EditUI.xaml
    /// </summary>
    public partial class EditUI : Window
    {
        public MainWindow MnWindow { get; set; }
        public Storm XStorm { get; set; }
        public EditUI(Storm Storm)
        {
            InitializeComponent();
            XStorm = Storm;
            MnWindow = (MainWindow)Application.Current.MainWindow; 
        }

        private void Setup()
        {
            EditStorm_EditNameBox.Text = XStorm.Name;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Setup(); 
        }

        private void EditStorm_OKButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Storm CStorm in MnWindow.CurrentProject.SelectedBasin.Storms)
            {
                if (CStorm.Id == XStorm.Id)
                {
                    CStorm.Name = EditStorm_EditNameBox.Text; // set the name.
                     
                }
            }


            Close();

        }
    }
}
