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
    /// Interaction logic for EditUI.xaml
    /// </summary>
    public partial class EditUI : UserControl
    {
        public string StormName { get; set; }
        public EventHandler<DanoEventArgs> DoneHit { get; set; }
        public EditUI()
        {
            InitializeComponent();
        }

        private protected void Init()
        {
            EditStorm_EditNameBox.DataContext = this;
        }

        private void EditStorm_OKButton_Click(object sender, RoutedEventArgs e)
        {
            DanoEventArgs DEA = new DanoEventArgs();
            DEA.DanoParameters.Add(EditStorm_EditNameBox.Text);
            DoneHit(this, DEA); 
        }
    }
}
