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
    /// PLS HAVE A WALK TODAY
    /// <summary>
    /// Track Maker 2.0 "Dano" StartPage
    /// </summary>
    public partial class StartPage : UserControl
    {
        public EventHandler CreateAnimationHit { get; set; }
        public EventHandler CreateTrackHit { get; set; }
        public EventHandler GetOnlineHit { get; set; }
        public EventHandler PreferencesHit { get; set; }
        public EventHandler StartPageExit { get; set; }
        public EventHandler TipsTextPreload { get; set; }
        public EventHandler WhatsNewTextPreload { get; set; }
        public StartPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raises the start page exit event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            EventHandler _test = StartPageExit;
            _test.Invoke(sender, e);
        }

        /// <summary>
        /// Raises the create track pressed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DanoUI_StartPage_CreateTrack_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CreateTrackHit(sender, e);
        }

        /// <summary>
        /// Raises the create animation pressed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DanoUI_StartPage_CreateAnimation_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CreateAnimationHit(sender, e);
        }
    }
}
