using TrackMaker.Util.Core;
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
/// File modified: 2021-01-18       Iris v676
/// </summary>


namespace TrackMaker.Core
{
    /// <summary>
    /// A static class used for logging debug information.
    /// </summary>
    public static class Logging
    {
        public static string FileName { get; set; }
        private static string LogHeader = "TrackMaker Iris Debug:"; // log header

        public static void Init()
        {
            LogFile("Track Maker\n\n© 2019-2021 starfrost. Open-source software under the MIT License.", true);
        }

        public static void Log(string Text) // logs to file. 
        {

            Trace.WriteLine($@"{LogHeader} [{DateTime.Now}] {Text}");
            LogFile(Text, false); 

        }

        public static void LogFile(string Text, bool IsNew = false)
        {
            try
            {
                FileName = $"Iris-Log-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.txt";

                TemporaryFileSettings TFS = new TemporaryFileSettings();
                TFS.TemporaryFileLocation = ".";
                TFS.Name = FileName;
                TFS.DelayClearUntilNextStart = true; 

                if (IsNew)
                {
                    // Crippling this as we're integrating it with the track maker.


                    FileStream XF = GlobalState.TFM.CreateNewFile(TFS);
                    using (StreamWriter SW = new StreamWriter(XF))
                    {
                        SW.BaseStream.Seek(0, SeekOrigin.End);
                        SW.WriteLine($@"{LogHeader} [{DateTime.Now}] - {Text}");
                    }
                }
                else
                {
                    FileStream XF = GlobalState.TFM.CreateNewFile(TFS);
                    using (StreamWriter SW = new StreamWriter(XF)) // OpenOrCreate just in case
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
                MessageBox.Show($"Error 301 (an error occurred writing to the log - Log not found", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
#endif
            }
        }
    }
}
