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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for LayerControlHost.xaml
    /// </summary>
    public partial class LayerControlHost : UserControl
    {

        public void AddLayer(string Name) => Layers.LayerNames.Add(Name);
        public void RemoveLayer(string Name) => Layers.LayerNames.Remove(Name);

        public LayerControlHost()
        {
            InitializeComponent();
        }
    }
}
