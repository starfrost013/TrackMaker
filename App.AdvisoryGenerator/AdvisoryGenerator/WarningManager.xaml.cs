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

namespace Dano.AdvisoryGenerator
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class WarningManager : Window
    {

        public AdvMainWindow MnWindow;

        public WarningManager(AdvMainWindow MainWindow)
        {
            InitializeComponent();
            MnWindow = MainWindow; // yes 
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Warning Warning = new Warning { Type = "Severe thunderstorm watch", IssuingAgency = "Fuck the Shit Centre", AreaFrom = "a", AreaTo = "b", Text = "you will die" };
            Warning.Type = TypeBox.Text;
            Warning.AreaFrom = FromBox.Text;
            Warning.AreaTo = ToBox.Text;
            Warning.IssuingAgency = IssuingAgencyBox.Text;
            
            if (WarningTextBox.Text == "") // Warning text box
            {
                Warning.Text = "None";
            }
            else
            {
                Warning.Text = WarningTextBox.Text; // set it to the text if none
            }
            Warnings.Items.Add(Warning);
            MnWindow.WarningList.Add(Warning);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // CLOSE
        }
    }
}
