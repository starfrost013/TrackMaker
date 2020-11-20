using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Probably will move to Useful Library 5.3.0
/// </summary>
namespace ExceptionsLite2.Base
{
    /// <summary>
    /// There is no UI-framework independent API to bring up a message box,
    /// so we are calling Win32.
    /// </summary>
    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern int MessageBox(int HWnd, string Text, string Caption, uint Type); 
    }
}
