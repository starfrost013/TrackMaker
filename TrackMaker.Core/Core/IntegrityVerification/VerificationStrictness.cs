using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Core
{
    /// <summary>
    /// Track Maker 2.1
    /// 
    /// Software Integrity Verification (Iris v719 - 2020-02-15)
    /// 
    /// VerificationStrictness.cs
    /// 
    /// Handles the strictness with which to verify software integrity
    /// </summary>
    public enum VerificationStrictness
    {
        /// <summary>
        /// Mild integrity verification. Only validate the existence of the file.
        /// </summary>
        Low = 0,

        /// <summary>
        /// Moderate integrity verification. Verify that the file is not zero-length or otherwise corrupted.
        /// </summary>
        Moderate = 1,

        /// <summary>
        /// Strict integrity verification. Verify the SHA256 hash of this file.
        /// </summary>
        Strict = 2

    }
}
