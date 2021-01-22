using Microsoft.Win32; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiralen.UpdateServices.ApplicationSettings
{
    /// <summary>
    /// Update Services setting
    /// </summary>
    public static class UpdaterApplicationSettings
    {
        /// <summary>
        /// The update server to use for acquiring updates
        /// </summary>
        public static string UpdateServer = "https://v2.trackmaker-update.medicanecentre.org";

        /// <summary>
        /// The currently selected update channel. 
        /// </summary>
        public static UpdateChannel UpdateChannel { get; set; }

        public static bool OptedIn { get; set; }

        public static long InstallationID { get; set; }

        public static void FirstTimeSetup()
        {
            HaveALittleFun(); 
            
           
        }

        /// <summary>
        /// Have a little fun with the InstallationID...I'm feeling like torture tonight (2020/09/25)
        /// </summary>
        public static void HaveALittleFun()
        {
            Random Rnd = new Random();
            long prec = Rnd.Next(100000000, 999999999);

            if (prec < 100190000)
            {
                InstallationID = 0xD15EA5E + (7551216 - 15190420 + 18330328 + 18761117 + 20010911 - 20050707) + prec;
                return; 
            }

            long prec2 = prec ^ 0xDEAD;

            prec2 *= 2;

            // Eight tragedies
            DateTime Tragedy_ALR = new DateTime(755, 12, 16); // An Lushan Rebellion
            DateTime Tragedy_EFA = new DateTime(1519, 4, 20); // European first contact with the Aztecs
            DateTime Tragedy_TOT = new DateTime(1833, 3, 28); // Forced signing of land removal as part of the Trail of Tears
            DateTime Tragedy_C77 = new DateTime(1876, 11, 7); // Compromise of 1877
            DateTime Tragedy_WW1 = new DateTime(1914, 6, 28); // Imperial clownery
            DateTime Tragedy_HIT = new DateTime(1933, 1, 30); // Hitler becomes chancellor
            DateTime Tragedy_HOS = new DateTime(1945, 8, 6); // Hiroshima
            DateTime Tragedy_911 = new DateTime(2001, 9, 11); // 9/11
            DateTime Tragedy_GFY = new DateTime(2020, 5, 26); // George Floyd

            DateTime CurrentEvent = new DateTime();

            int CurrentNext = Rnd.Next(0, 1000);

            if (CurrentNext <= 25) CurrentEvent = Tragedy_ALR;
            if (CurrentNext > 25 && CurrentNext <= 50) CurrentEvent = Tragedy_EFA;
            if (CurrentNext > 50 && CurrentNext <= 75) CurrentEvent = Tragedy_TOT;
            if (CurrentNext > 75 && CurrentNext <= 100) CurrentEvent = Tragedy_C77;
            if (CurrentNext > 100 && CurrentNext <= 125) CurrentEvent = Tragedy_WW1;
            if (CurrentNext > 125 && CurrentNext <= 150) CurrentEvent = Tragedy_HIT;
            if (CurrentNext > 150 && CurrentNext <= 175) CurrentEvent = Tragedy_HOS;
            if (CurrentNext > 175 && CurrentNext <= 200) CurrentEvent = Tragedy_911;
            if (CurrentNext > 200) CurrentEvent = Tragedy_GFY; 

            InstallationID = prec2 + CurrentEvent.Ticks; 

            
                
        }
    }
}
