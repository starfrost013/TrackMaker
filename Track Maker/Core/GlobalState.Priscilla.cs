using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    /// <summary>
    /// Priscilla   v514 (pre-beta release 3 stage)
    /// 
    /// GlobalState.Priscilla
    /// 
    /// Version 2.0 Global State
    /// 
    /// 10/31/20
    /// </summary>
    public class GlobalStateP // move to Starfrost UL5 Version 5.3?
    {
        public static string CurrentlyOpenFile { get; set; }

        public void SetCurrentOpenFile(string FileName) => CurrentlyOpenFile = FileName;
        public string GetCurrentOpenFile() => CurrentlyOpenFile;
    }
}
