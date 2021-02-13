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
using TrackMaker.Core;

namespace TrackMaker
{
    /// <summary>
    /// Interaction logic for DanoCategoryManagerHost.xaml
    /// </summary>
    public partial class DanoCategoryManagerHost : Window
    {
        public DanoCategoryManagerHost(List<string> SystemList, List<string> NameList)
        {
            InitializeComponent();

            // This can be improved (Priscilla 428)
            
            Dano_CategoryManagerUC.BasinStrings = SystemList;
            Dano_CategoryManagerUC.CategoryStrings = NameList; 
            Dano_CategoryManagerUC.UpdateLayout();
            
        }

        private void CloseHit(object sender, DanoEventArgs e)
        {
            try
            {
                Debug.Assert(e.DanoParameters.Count == 2);

                bool HasChanged = Convert.ToBoolean(e.DanoParameters[0]);
                string NewCategoryName = e.DanoParameters[1].ToString();

                // Temp (move to basin submethod)
                MainWindow MnWindow = (MainWindow)Application.Current.MainWindow;
                GlobalState.CategoryManager.SetCategoryWithName(NewCategoryName);
                Close();
            }
            catch (FormatException err)
            {

#if DEBUG
                Error.Throw($"An internal error occurred while converting DanoEventArg parameters (DanoCategoryManagerHost.xaml.cs)\n\n{err}", "IE100", ErrorSeverity.FatalError, 100);
#else
                Error.Throw("An internal error occurred while converting DanoEventArg parameters.", "IE100", ErrorSeverity.FatalError, 100);
#endif
            }
        }
    }
}
