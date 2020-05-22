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
    /// PLS HAVE A WALK TOMORROW
    /// <summary>
    /// Track Maker 2.0 "Dano" StartPage
    /// </summary>
    public partial class StartPage : UserControl
    {
        public EventHandler StartPageExit { get; set; }
        public StartPage()
        {
            InitializeComponent();
        }
    }
}
