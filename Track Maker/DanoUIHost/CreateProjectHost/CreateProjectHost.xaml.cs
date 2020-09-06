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

//Move to DanoUI.Host?
namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for CreateProjectHost.xaml
    /// </summary>
    public partial class CreateProjectHost : Window
    {
        public CreateProjectHost(List<string> BasinList)
        {
            InitializeComponent();
            DNPHost.Dano_NewProject_InitDBL(BasinList); 
            DNPHost.Dano_NewProject_Init(); 
        }

        public void NewProjectCreated(object sender, DanoEventArgs e)
        {
            // Version 1.5 'Priscilla'

            // Implement globalstate after this
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
            Project TProj = new Project();

            TProj.LoadBasins(); 
            // Create the basins
            Basin CBasin = TProj.GetBasinWithName((string)e.DanoParameters[0]);

            TProj.AddBasin(CBasin.Name, CBasin.BasinImagePath); 


            MnWindow.CurrentProject = TProj;
            
            // Load basin


            // Temp
            MnWindow.EnableButtons(); 
            Close();
        }
    }
}
