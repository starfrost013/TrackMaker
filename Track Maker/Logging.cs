using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Cosmo's Track Maker
/// 
/// Logging.cs
/// 
/// Purpose: Logging events that occur to a text file.
/// 
/// File created: 2019-11-09
/// </summary>


namespace Track_Maker
{
    class Logging
    {
        public static void Log(string text) // logs to file. 
        {
            if (MainWindow.Debug > 0)
            {
                Trace.WriteLine($"Debug Collective: [{DateTime.Now}] {text}");
            }
        }
    }
}
