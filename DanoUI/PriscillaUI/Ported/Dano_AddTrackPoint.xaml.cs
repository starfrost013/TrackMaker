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
    /// Interaction logic for Dano_AddTrackPoint.xaml
    /// </summary>
    public partial class AddTrackPoint : UserControl
    {
        public List<string> TypeNames { get; set; }
        public EventHandler<DanoEventArgs> OKHit { get; set; }
        public AddTrackPoint()
        {
            InitializeComponent();

            UpdateLayout(); 
        }

        private void TypeSelect_Loaded(object sender, RoutedEventArgs e)
        {
            TypeSelect.Setup(TypeNames); 
        }
    }
}
