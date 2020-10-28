using Starfrost.UL5.Logging; 
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

namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for TypeSelectorBox.xaml
    /// </summary>
    public partial class TypeSelectorBox : UserControl
    {
        public MainWindow MainWindow { get; set; }
        public TypeSelectorBox()
        {
            InitializeComponent();
        }

        private void Setup()
        {
#if PRISCILLA
            MainWindow = (MainWindow)Application.Current.MainWindow; //figure something else out
            List<StormType2> ST2L = MainWindow.ST2Manager.GetListOfStormTypes(); 

#else
            List<StormType2> ST2L = GlobalState.GetST2Manager(); 
#endif
            //var StormTypes = Enum.GetValues(typeof(StormType));


            Logging.Log("Passed MainWindow");

            foreach (StormType2 ST2 in ST2L)
            {
                TypeBox.Items.Add(ST2.Name); 
            }

            Logging.Log("Populated TypeBox...");
            if (TypeBox.Items.Count < 1)
            {
                Error.Throw("Uh oh", "Something bad happened. The TypeBox failed to populate.", ErrorSeverity.FatalError, 0);
            }

            Logging.Log("Setting selected index...");
            TypeBox.SelectedIndex = 0;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Setup(); 
        }
    }
}
