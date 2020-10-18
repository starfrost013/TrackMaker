using BetterWin32Errors; 
using Starfrost.UL5.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace Starfrost.UL5.Win32X
{
    /// <summary>
    /// Win32 Native Methods
    /// 
    /// To reference a native method: the extern declaration should be private, and you should create a *function name*__Managed function for public APIs.
    /// Callbacks should also be private and located in this class.
    /// You should also call LogLeavingCLR(), LogEnteringCLR(), and CheckLastWin32Error() in your __Managed function. If you need to handle Win32 errors (like error 2/5/whatever),
    /// then handle those before calling CheckLastWin32Error().
    /// 
    /// May create CheckLastWin32Error(int W32Error); for this case.
    /// </summary>
    public class NativeMethods
    {
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(SystemMetric SysMetric); // temp

        /// <summary>
        /// Native method wrapper for GetSystemMetrics [Win32 API] 
        /// </summary>
        /// <param name="SysMetric"></param>
        /// <returns></returns>
        public static int GetSystemMetrics__Managed(SystemMetric SysMetric)
        {
            LogLeavingCLR();
            int Result = GetSystemMetrics(SysMetric); 
            LogEnteringCLR();
            CheckLastWin32Error(); 

            return Result;
        }

        private static void LogEnteringCLR() => DbgLogging.Log("Now entering managed code - Win32 function has been called");
        private static void LogLeavingCLR() => DbgLogging.Log("Now exiting managed code - calling Win32 function");

        private static void CheckLastWin32Error()
        {
            int LastW32Error = Marshal.GetLastWin32Error();
            int HResult = Marshal.GetHRForLastWin32Error();

            if (LastW32Error != 0)
            {
                MessageBox.Show($"Error 0x5555dead (Internal): A win32 error has occurred - {LastW32Error} (Code: {(Win32Error)LastW32Error}, HRESULT: {HResult}");
#if DEBUG
                return; // try to recover
#else
                // this is dumb but we don't know what has occurred
                Environment.Exit(0x5555dead); 
#endif
            }
        }

    }
}
