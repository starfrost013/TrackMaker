using Dano.ACECalculator;
using Dano.AdvisoryGenerator;
using TrackMaker.UI.StringUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        /// Perhaps expand this in v3.
        /// </summary>
        public enum ParseArgResult
        { 
            // Default init
            DoNothing = 0, 

            // Initialise the ACE Calc at startup instead. (can also be done in settings) 
            InitACECalc = 1,

            // Initialise the Advisory Generator at startup instead. (can also be done in settings) 
            InitAdvGen = 2
        }

        /// <summary>
        /// Application Startup Phase 0
        /// 
        /// Initialise window; verify integrity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {

            // Initialise different mainwindows depending on the provided arguments.
            switch (ParseArgs(e.Args))
            {
                case ParseArgResult.DoNothing:
                    MainWindow MnWindow = new MainWindow();
                    MnWindow.Show();
                    return; 
                case ParseArgResult.InitACECalc:
                    CalcMainWindow CalcMainWindow = new CalcMainWindow();
                    CalcMainWindow.Show();
                    return;
                case ParseArgResult.InitAdvGen:
                    AdvMainWindow AMainWindow = new AdvMainWindow();
                    AMainWindow.Show();
                    return; 
            }

        }

        private ParseArgResult ParseArgs(string[] Args)
        {
            // If there are no arguments, do nothing...

            // v549: rewritten to be more versatile
            switch (Args.Length)
            {
                case 0:
                    return ParseArgResult.DoNothing;
                default:
                    foreach (string Argument in Args)
                    {
                        if (Argument.ContainsCaseInsensitive("-initacecalc")) return ParseArgResult.InitACECalc;
                        if (Argument.ContainsCaseInsensitive("-initadvgen")) return ParseArgResult.InitAdvGen;
                    }

                    return ParseArgResult.DoNothing;
            }

        }

    }
}
