using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Track_Maker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Dano init.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Started(object sender, EventArgs e)
        {
            // Settings haven't been loaded. If we're compiling for Dano, run this.
            MainWindow MnWindow = new MainWindow();
            MnWindow.Show();
#if DANO
            StartPageHost SPH = new StartPageHost();
            SPH.Show();

#endif
        }
    }
}
