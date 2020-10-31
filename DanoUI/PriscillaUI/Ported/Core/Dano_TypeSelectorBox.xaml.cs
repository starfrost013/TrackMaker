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
    /// DanoUI/PriscillaUI/Ported/Core/Dano_TypeSelectorBox.xaml.cs
    /// 
    /// © 2019 - 2020 starfrost (Version 2.00) for Priscilla v507+
    /// </summary>
    public partial class TypeSelectorBox : UserControl
    {
        public List<string> StormNames { get; set; }
        public string SelectedItem { get; set; } // easily get the selected item
        public TypeSelectorBox()
        {
            InitializeComponent();
        }

        public bool Setup(List<string> StormTypeNames) // pre-componentisation 
        {
#if DANO
            List<StormType2> ST2L = GlobalState.GetST2Manager(); 
#endif
            // BAD / UNFINISHED CODE BEGIN

            // we need to overhaul this around beta to use bindings 
            // and also logging and errors because this is T E R R I B L E !
            foreach (string ST2 in StormTypeNames)
            {
                TypeBox.Items.Add(ST2);
            }

            //Logging.Log("Populated TypeBox...");
            if (TypeBox.Items.Count < 1)
            {
                MessageBox.Show("Error - failed to populate TypeBox!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            //Logging.Log("Setting selected index...");
            TypeBox.SelectedIndex = 0;
            UpdateLayout();

            // BAD / UNFINISHED CODE END

            return true; 
        }

        private void TypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => SelectedItem = TypeBox.Text;
    }
}
