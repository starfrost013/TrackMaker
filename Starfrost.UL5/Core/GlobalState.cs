using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfrost.UL5.Core
{
    /// <summary>
    /// Starfrost Useful Library v5.2+ GlobalState
    /// </summary>
    public class GlobalState
    {
        private static App CApp { get; set; }

        /// <summary>
        /// Determines if this application has reached end of support.
        /// </summary>
        public static bool EoS => true; 
        public static void V52_Init(string AppName)
        {
            CApp = AppRegistration.RegisterApp(AppName); 
        }

        public static App GetApp() => CApp;
        public static string GetAppName() => CApp.Name;
    }
}
