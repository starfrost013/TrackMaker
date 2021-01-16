using TrackMaker.UI.StringUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms; 
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Track_Maker

{
    /// <summary>
    /// Interaction logic for ColourPicker.xaml
    /// 
    /// Custom WPF colo(u)r picker control
    /// 
    /// 2020-05-10 for Track Maker 0.9 
    /// 
    /// Modified 2020-05-19 for Track Maker 1.0; enhanced to show the currently selected colour at start up.
    /// </summary>
    public partial class ColourPicker : System.Windows.Controls.UserControl
    {
        public Color SelectedColour { get; set; }
        public ColourPicker()
        {
            InitializeComponent();
            UpdateRectangle(); 
        }

        public void ShowDialog()
        {
            ColorDialog _ = new ColorDialog();
            try
            { 
                _.FullOpen = true;

                _.Color = Utilities.ConvertWpfToWinformsColour(SelectedColour);

                if (_.ShowDialog() == DialogResult.OK)
                {
                    SelectedColour = Utilities.ConvertWinformsToWpfColour(_.Color); // Convert from WinForms to WPF colour

                    // set the colour
                    UpdateRectangle();


                    return; // return to caller. 
                }
                else // User hit cancel
                {
                    return;
                }
            }
            finally
            {
                _.Dispose();
            }
        }

        internal void UpdateRectangle()
        {
            PickColourButton.Fill = new SolidColorBrush(SelectedColour);
            UpdateLayout(); 
        }
    }
}
