using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Starfrost.UL5.PlatformUtilities
{
    /// <summary>
    /// Valid Tiralen platforms.
    /// 
    /// Sysreqs: (as of 8/24/20)
    /// .NET Core 3.1
    /// 
    /// Windows: Windows 7 Service Pack 1 or later (excluding Windows 8 and Wnidows 10 Threshold)
    /// Mac OS: 10.13+ (for x64), 11.0+ (for ARM, not now but when .NET supports it)
    /// Linux: RHEL 6+, CentOS 7+, Oracle Linux 7+, Fedora 30+, Debian 9+, Ubuntu 16.04+, Mint 18+, openSUSE 15+,
    /// SLES 12 Service Pack 2+, and Alpine Linux 3.10+
    /// </summary>
    public enum TiralenPlatforms
    { 
        // 32-bit WIndows 7 Service Pack 1, Windows 8.1, and Windows 10 v1607 
        Win32,

        // 64-bit Windows 7 Service Pack 1, Windows 8.1, and Windows 10 v1607+
        Win64, 
        
        // Mac OS 10.13 and later, x64
        MacOS64, 
        
        // Mac OS 11.x, Arm64 (for when we update to newer .NET in the future)
        MacOSARM64, 
        
        // 32-bit Linux
        Linux32, 
        
        // 64-bit Linux
        Linux64, 
    }

    public static class TiralenPlatform
    {
        /// <summary>
        /// What are we running on?
        /// </summary>
        /// <returns></returns>
        public static TiralenPlatforms DetermineRuntimePlatform()
        {
            if (!IsWindows())
            {
                if (!IsMacOS())
                {
                    if (!IsX64())
                    {
                        return TiralenPlatforms.Linux64;
                    }
                    else
                    {
                        return TiralenPlatforms.Linux32;
                    }
                }
                else
                {
                    if (!IsX64())
                    {
                        return TiralenPlatforms.MacOSARM64;
                    }
                    else
                    {
                        return TiralenPlatforms.MacOS64;
                    }
                }
            }
            else
            {
                // 32 bit windows
                if (!IsX64())
                {
                    return TiralenPlatforms.Win32;
                }
                else
                {
                    return TiralenPlatforms.Win64;
                }
            }
        }

        private static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private static bool IsMacOS() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        private static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        private static bool IsX64() => (RuntimeInformation.ProcessArchitecture == Architecture.X64);
        private static bool IsArm64() => (RuntimeInformation.ProcessArchitecture == Architecture.Arm64);
    }
}
