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
    /// Interaction logic for Priscilla_QualityControl.xaml
    /// </summary>
    public partial class QualityControl : UserControl
    {
        public EventHandler<DanoEventArgs> FullQualityHit { get; set; }
        public EventHandler<DanoEventArgs> HalfQualityHit { get; set; }
        public EventHandler<DanoEventArgs> QuarterQualityHit { get; set; }
        public EventHandler<DanoEventArgs> EighthQualityHit { get; set; }

#if DANO
        public EventHandler<DanoEventArgs> CustomQualityHit { get; set; }

        public EventHandler<DanoEventArgs> NearestNeighbourHit {get; set; }
        
        public EventHandler<DanoEventArgs> BilinearHit {get; set; }
        
        public EventHandler<DanoEventArgs> FantHit {get; set; }
#endif
        public QualityControl()
        {
            InitializeComponent();
        }

        private void QualityControl_FullQuality_Click(object sender, RoutedEventArgs e)
        {
            QualityControl_FullQuality.IsChecked = true;
            QualityControl_HalfQuality.IsChecked = false;
            QualityControl_QuarterQuality.IsChecked = false;
            QualityControl_EighthQuality.IsChecked = false;
        }

        private void QualityControl_HalfQuality_Click(object sender, RoutedEventArgs e)
        {
            QualityControl_FullQuality.IsChecked = false;
            QualityControl_HalfQuality.IsChecked = true;
            QualityControl_QuarterQuality.IsChecked = false;
            QualityControl_EighthQuality.IsChecked = false;
        }

        private void QualityControl_QuarterQuality_Click(object sender, RoutedEventArgs e)
        {
            QualityControl_FullQuality.IsChecked = false;
            QualityControl_HalfQuality.IsChecked = false;
            QualityControl_QuarterQuality.IsChecked = true;
            QualityControl_EighthQuality.IsChecked = false;
        }

        private void QualityControl_EighthQuality_Click(object sender, RoutedEventArgs e)
        {
            QualityControl_FullQuality.IsChecked = false;
            QualityControl_HalfQuality.IsChecked = false;
            QualityControl_QuarterQuality.IsChecked = false;
            QualityControl_EighthQuality.IsChecked = true;
        }
    }
}
