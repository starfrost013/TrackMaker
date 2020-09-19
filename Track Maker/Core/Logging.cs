using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// starfrost's Track Maker
/// 
/// Logging.cs
/// 
/// Purpose: Logging events that occur to a text file.
/// 
/// File created: 2019-11-09
/// 
/// File modified: 2020-09-20 
/// </summary>


namespace Track_Maker
{
    public class Logging
    {
        public static void Log(string text) // logs to file. 
        {

#if DEBUG
            Trace.WriteLine($"Debug Collective: [{DateTime.Now}] {text}");
#endif

        }
    }
}
