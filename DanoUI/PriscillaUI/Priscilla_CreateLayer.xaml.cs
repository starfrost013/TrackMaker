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
    /// Interaction logic for CreateLayer.xaml
    /// </summary>
    public partial class CreateLayer : UserControl
    {
        
        public EventHandler<DanoEventArgs> BringToFrontHit { get; set; }
        public EventHandler<DanoEventArgs> BringToBackHit { get; set; }
        public EventHandler<DanoEventArgs> OKHit { get; set; }
        public CreateLayer()
        {
            InitializeComponent();
        }
    }
}
