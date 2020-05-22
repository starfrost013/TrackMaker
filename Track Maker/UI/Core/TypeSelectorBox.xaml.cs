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
            var StormTypes = Enum.GetValues(typeof(StormType));

            string ToAdd;

            MainWindow = (MainWindow)Application.Current.MainWindow; //figure something else out

            Logging.Log("Passed MainWindow");
            foreach (StormType stormtype in StormTypes)
            {
                Logging.Log($"Populating TypeBox with {stormtype.ToString()}");
                switch (stormtype)
                {
                    case StormType.Tropical:
                        ToAdd = "Tropical cyclone";
                        TypeBox.Items.Add(ToAdd);
                        continue;
                    case StormType.Subtropical:
                        ToAdd = "Subtropical cyclone";
                        TypeBox.Items.Add(ToAdd);
                        continue;
                    case StormType.Extratropical:
                        ToAdd = "Extratropical cyclone";
                        TypeBox.Items.Add(ToAdd);
                        continue;
                    case StormType.InvestPTC:
                        ToAdd = "Invest / PTC";
                        TypeBox.Items.Add(ToAdd);
                        continue; 
                    case StormType.PolarLow:
                        ToAdd = "Polar low";
                        TypeBox.Items.Add(ToAdd); // maybe worse?
                        continue;
                }
            }

            if (TypeBox.Items.Count < 1)
            {
                Error.Throw("Uh oh", "Something bad happened. The TypeBox failed to populate.", Error.ErrorSeverity.FatalError, 0);
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
