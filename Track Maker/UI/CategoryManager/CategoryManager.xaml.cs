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

namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for CategoryManager.xaml
    /// </summary>
    public partial class CategoryManagerWindow : Window
    {
        public MainWindow MnWindow { get; set; }
        public CategoryManagerWindow()
        {
            InitializeComponent();
            MnWindow = (MainWindow)Application.Current.MainWindow; // yup
            
        }

        /// <summary>
        /// Sets up this window. 
        /// </summary>
        public void Setup()
        {
            Catman_SystemsBox.DataContext = MnWindow.Catman.CategorySystems;
            Catman_CatsBox.DataContext = MnWindow.Catman.CurrentCategorySystem.Categories;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Setup();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }

        private void Catman_SystemsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < MnWindow.Catman.CategorySystems.Count; i++)
            {
                CategorySystem Catsystem = MnWindow.Catman.CategorySystems[i];

                // set the category system category list to the selected category system
                if (Catman_SystemsBox.SelectedIndex == i)
                {
                    MnWindow.Catman.CurrentCategorySystem = Catsystem;
                    Catman_CatsBox.DataContext = Catsystem.Categories;
                }
            }
        }
    }
}
