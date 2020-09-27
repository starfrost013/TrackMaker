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
    /// Interaction logic for Dano_SeasonManager.xaml
    /// </summary>
    public partial class SeasonManager : UserControl
    {
        public List<string> StormNameList { get; set; }

        /// <summary>
        /// DanoEventArgs[0] - ID of storm selected within flat list of storms
        /// </summary>
        public EventHandler<DanoEventArgs> EditHit { get; set; }
        public EventHandler<DanoEventArgs> RenameHit { get; set; }
        public EventHandler<DanoEventArgs> DeleteHit { get; set; }
        public EventHandler<DanoEventArgs> OKHit { get; set; }
        public SeasonManager()
        {
            InitializeComponent();
        }

        public void Init()
        {
            StormList.DataContext = this; 
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Init(); 
        }

        public void EditButton_Click(object sender, RoutedEventArgs e)
        {
            DanoEventArgs DEA = new DanoEventArgs();
            DEA.DanoParameters.Add(StormList.SelectedItem);
            EditHit(this, DEA);
        }

        public void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            DanoEventArgs DEA = new DanoEventArgs();
            DEA.DanoParameters.Add(StormList.SelectedItem);
            EditHit(this, DEA);
        }

        public void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DanoEventArgs DEA = new DanoEventArgs();
            DEA.DanoParameters.Add(StormList.SelectedItem);
            DeleteHit(this, DEA); 
        }

        public void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DanoEventArgs DEA = new DanoEventArgs();
            DEA.DanoParameters.Add(StormList.SelectedItem);
            OKHit(this, DEA); 
        }
    }
}
