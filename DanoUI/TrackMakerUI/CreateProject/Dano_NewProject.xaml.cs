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

namespace TrackMaker.UI
{
    /// <summary>
    /// Interaction logic for Dano_NewProject.xaml
    /// </summary>
    public partial class Dano_NewProject : UserControl
    {
        public EventHandler<DanoEventArgs> NewProjectCreated { get; set; }
        public EventHandler<DanoEventArgs> DanoOnCreate { get; set; }
        public List<string> DanoBasinList { get; set; }

        public void Dano_NewProject_InitDBL(List<string> DBL)
        {
            DanoBasinList = DBL;
        }

        // Bit of a hack.
        public void Dano_NewProject_Init()
        {
            InitializeComponent();
            Dano_UI_CreateProject_InitialBasinBox.DataContext = this;
        }

        private void Dano_UI_CreateProject_Create_Click(object sender, RoutedEventArgs e)
        {

            if (Dano_UI_CreateProject_InitialBasinBox.SelectedIndex == -1)
            {
                MessageBox.Show("You must select a basin!");
                return;
            }

            DanoEventArgs DEA = new DanoEventArgs();

            DEA.DanoParameters.Add(Dano_UI_CreateProject_NameBox.Text);
            DEA.DanoParameters.Add(Dano_UI_CreateProject_InitialBasinBox.Text);
            NewProjectCreated.Invoke(sender, DEA); 
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Dano_UI_CreateProject_InitialBasinBox.DataContext = this;
            UpdateLayout();
        }
    }
}
