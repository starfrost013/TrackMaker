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
        public MainWindow TEMP3_WILLBEREMOVEDWITHPROJECT { get; set; }
        public DanoBasinSwitcherHost(List<string> XStrList)
        {
            InitializeComponent();
            Dano_BasinSwitcherUserControl.BasinString = XStrList;
            Dano_BasinSwitcherUserControl.UpdateLayout();
            TEMP3_WILLBEREMOVEDWITHPROJECT = (MainWindow)Application.Current.MainWindow; 
        }

        // Move to Project.SetCurrentBasin() for Dano Milestone 2.
        public void Temp2(string CurBasinName)
        {
            foreach (Basin Basin in TEMP3_WILLBEREMOVEDWITHPROJECT.CurrentProject.Basins)
            {
                if (Basin.Name == CurBasinName)
                {
                    TEMP3_WILLBEREMOVEDWITHPROJECT.CurrentProject.SelectedBasin = Basin;
                    // Remove hack code.[m2]
                    TEMP3_WILLBEREMOVEDWITHPROJECT.HurricaneBasin.Background = new ImageBrush(new BitmapImage(new Uri(Basin.BasinImagePath, UriKind.RelativeOrAbsolute)));
                }
            }

        }

        // Event Handler for    Dano UI BasinSwitcher UserControl Closed event

        public void UC_Closed(object sender, DanoEventArgs e)
        {
            Temp2((string)e.DanoParameters[0]); 
            Close();
        }
    }
}
