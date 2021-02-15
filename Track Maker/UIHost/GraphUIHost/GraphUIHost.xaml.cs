using TrackMaker.Core.Graphing;
using TrackMaker.UI;
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

namespace TrackMaker
{
    /// <summary>
    /// Interaction logic for GraphUIHost.xaml
    /// </summary>
    public partial class GraphUIHost : Window
    {
        public GraphUIHost()
        {
            InitializeComponent();
        }

        public void DisplayButtonHit(object Sender, DanoEventArgs DEA)
        {
            Debug.Assert(DEA.DanoParameters.Count == 1);

            StormGraph SG = (StormGraph)DEA.DanoParameters[0];

            // if (!SG.Show())

            GraphDisplayHost GDH = new GraphDisplayHost(SG);
            GDH.Owner = this;
            GDH.Show();
        }
    }
}
