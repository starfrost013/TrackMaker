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
    /// 
    /// DanoUI version of the Category Manager.
    /// 
    /// 2020/09/03 [Priscilla build 427]
    /// 
    /// </summary>
    public partial class DanoCategoryManager : UserControl
    {
        public List<string> BasinStrings { get; set; }

        /// <summary>
        /// Event arg 0 (bool) - was changed
        /// </summary>
        public EventHandler<DanoEventArgs> CloseHit { get; set; }
        public DanoCategoryManager()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DanoEventArgs DEA = new DanoEventArgs();

            bool HasChanged = false;

            if (Catman_SystemsBox.SelectedIndex != -1)
            {
                HasChanged = true;    
            }

            ListBoxItem SelectedCategorySystem = (ListBoxItem)Catman_SystemsBox.Items[Catman_SystemsBox.SelectedIndex];

            DEA.DanoParameters.Add(HasChanged);
            DEA.DanoParameters.Add(SelectedCategorySystem.Content);
            RaiseCloseHit(sender, DEA); 
        }

        private protected void RaiseCloseHit(object sender, DanoEventArgs e)
        {
            CloseHit(sender, e); 
        }
    }
}
