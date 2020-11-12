using DanoUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        /// Temp
        /// </summary>
        public Project TProj { get; set; }
        public CreateProjectHost(List<string> BasinList)
        {
            InitializeComponent();
            DNPHost.Dano_NewProject_InitDBL(BasinList); 
            DNPHost.Dano_NewProject_Init();

            // Constructor loads basins
            TProj = new Project(true);
        }

        public void NewProjectCreated(object sender, DanoEventArgs e)
        {
            // Version 1.5 'Priscilla'

            // Implement globalstate after this
#if PRISCILLA
            MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
#endif

            // Create the basins

            Debug.Assert(e.DanoParameters.Count == 2);

            TProj.AddBasin((string)e.DanoParameters[1], true); 


            MnWindow.CurrentProject = TProj;
            UpdateLayout();
            // Load basin


            // Temp
            MnWindow.EnableButtons(); 
            Close();
        }
    }
}
