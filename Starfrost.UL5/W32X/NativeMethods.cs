using BetterWin32Errors;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace TrackMaker.Util.Win32X
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
        public static extern int GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(SystemMetric SysMetric); // temp

        [DllImport("user32.dll")]
        public static extern int MessageBox(int HWnd, string Text, string Caption, uint Type);

        /// <summary>
        /// Native method wrapper for GetSystemMetrics [Win32 API] 
        /// </summary>
        /// <param name="SysMetric"></param>
        /// <returns></returns>
        public static int GetSystemMetrics__Managed(SystemMetric SysMetric)
        {
            //LogLeavingCLR();
            int Result = GetSystemMetrics(SysMetric); 
           // LogEnteringCLR();
            CheckLastWin32Error(); 

            return Result;
        }

        public static int MessageBox__Managed(string Text, string Caption, uint Type) // Enum required
        {
            //LogLeavingCLR();

            int HWND = GetForegroundWindow();
            CheckLastWin32Error();
            int MBResult = MessageBox(HWND, Text, Caption, Type);
            CheckLastWin32Error(); 
           // LogEnteringCLR();
            CheckLastWin32Error();

            return MBResult; 
        }

        /*
         * 
         * for now due to code restructuring this doesn't work
        private static void LogEnteringCLR() => Logging.Log("Now entering managed code - Win32 function has been called");
        private static void LogLeavingCLR() => Logging.Log("Now exiting managed code - calling Win32 function");
        */ 

        private static void CheckLastWin32Error()
        {
            int LastW32Error = Marshal.GetLastWin32Error();
            int HResult = Marshal.GetHRForLastWin32Error();

            if (LastW32Error != 0)
            {
                System.Windows.MessageBox.Show($"Error 0x5555dead (Internal): A Win32 error has occurred - {LastW32Error} (Code: {(Win32Error)LastW32Error}, HRESULT: {HResult}");
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
