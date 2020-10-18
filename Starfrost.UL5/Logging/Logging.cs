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
/// File modified: 2020-10-16   Priscilla v484
/// </summary>


namespace Starfrost.UL5.Logging
{
    /// <summary>
    /// A static class used for logging debug information.
    /// </summary>
    public static class DbgLogging
    {
        public static string FileName { get; set; }

        public static void Init()
        {
            LogFile("Track Maker\n\n© 2019-2020 starfrost. Open-source software under the MIT License.", true);
        }
        public static void Log(string Text) // logs to file. 
        {

#if DEBUG
            Trace.WriteLine($@"//Debug Collective//: [{DateTime.Now}] {Text}");
            LogFile(Text, false); 
#endif

        }

        public static void LogFile(string Text, bool IsNew = false)
        {
            try
            {
                if (IsNew)
                {
                    FileName = $"Priscilla-Log-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.txt";
                }

                // create the file
                using (StreamWriter SW = new StreamWriter(File.Create(FileName)))
                {
                    SW.BaseStream.Seek(0, SeekOrigin.End);
                    SW.WriteLine($@"//Debug Collective//: [{DateTime.Now}] - {Text}");
                }
            }
            catch (FileNotFoundException err)
            {
#if DEBUG
                Error.Throw("Error", $"An error has occurred writing to the log - the log file was not found!\n\n{err}", ErrorSeverity.Error, 172);
#else
                Error.Throw("Error", "An error has occurred writing to the log - the log file was not found!", ErrorSeverity.Error, 172);
#endif
            }
        }
    }
}
