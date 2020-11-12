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

namespace Track_Maker.DanoUIHost.AddTrackPointHost
{
    /// <summary>
    /// Interaction logic for AddTrackPointHost.xaml
    /// </summary>
    public partial class AddTrackPointHost : Window
    {
        public List<string> StormNameList { get; set; }
        public AddTrackPointHost(List<string> StormNames)
        {
            InitializeComponent();
            AddTrackPointControl.TypeNames = StormNames;
            StormNameList = StormNames;
        }

    }
}
