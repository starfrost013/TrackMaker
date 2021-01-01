using Starfrost.UL5.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

/// <summary>
/// starfrost's Track Maker
/// 
/// Logging.cs
/// 
/// Purpose: Logging events that occur to a text file.
/// 
/// File created: 2019-11-09
/// 
/// File modified: 2020-11-27   Priscilla v542
/// </summary>


namespace Starfrost.UL5.Logging
{
    /// <summary>
    /// A static class used for logging debug information.
    /// </summary>
    public static class Logging
    {
        public static string FileName { get; set; }
        private static string LogHeader = "Priscilla Debug:"; // log header
        public static void Init()
        {
            LogFile("Track Maker\n\n© 2019-2021 starfrost. Open-source software under the MIT License.", true);
        }

        public static void Log(string Text) // logs to file. 
        {

#if DEBUG
            Trace.WriteLine($@"{LogHeader} [{DateTime.Now}] {Text}");
            LogFile(Text, false); 
#endif

        }

        public static void LogFile(string Text, bool IsNew = false)
        {
            try
            {
                string AppName = GlobalState.GetAppName(); 
                if (IsNew)
                {
                    FileName = $"{AppName}-Log-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.txt";
                    using (StreamWriter SW = new StreamWriter(File.Create(FileName)))
                    {
                        SW.BaseStream.Seek(0, SeekOrigin.End);
                        SW.WriteLine($@"{LogHeader} [{DateTime.Now}] - {Text}");
                    }
                }
                else
                {
                    using (StreamWriter SW = new StreamWriter(File.Open(FileName, FileMode.OpenOrCreate))) // OpenOrCreate just in case
                    {
                        SW.BaseStream.Seek(0, SeekOrigin.End);
                        SW.WriteLine($@"{LogHeader} [{DateTime.Now}] - {Text}");
                    }
                }
                // create the file

            }
            catch (FileNotFoundException err)
            {
#if DEBUG
                MessageBox.Show($"Error 301 (an error occurred writing to the log - Log not found)\n\n{err}", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
#else
                MessageBox.Show($"Error 301 (an error occurred writing to the log - Log not found}", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
#endif
            }
        }
    }
}
