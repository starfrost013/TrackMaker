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

namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for DanoBasinSwitcherHost.xaml
    /// </summary>
    public partial class DanoBasinSwitcherHost : Window
    {
        public MainWindow MnWindow { get; set; }
        public DanoBasinSwitcherHost(List<string> XStrList)
        {
            InitializeComponent();
            Dano_BasinSwitcherUserControl.BasinString = XStrList;
            Dano_BasinSwitcherUserControl.UpdateLayout();
            MnWindow = (MainWindow)Application.Current.MainWindow; 
        }

        // Event Handler for    Dano UI BasinSwitcher UserControl Closed event

        public void UC_Closed(object sender, DanoEventArgs e)
        {
            string NewBasinName = (string)e.DanoParameters[0];
            Project Proj = MnWindow.CurrentProject;

            Proj.SelectBasin(NewBasinName);
            
            // I'd move it into the selectbasin method but this is code that shouldn't really exist anyway
            MnWindow.HurricaneBasin.Background = new ImageBrush(new BitmapImage(new Uri(Proj.SelectedBasin.ImagePath, UriKind.RelativeOrAbsolute)));
            Close();
        }

    }
}
