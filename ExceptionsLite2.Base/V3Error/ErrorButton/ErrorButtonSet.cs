using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionsLite2.Base
{
    /// <summary>
    /// Elm2 Button Set
    /// 
    /// These values have been specifically designed to minimise code use
    /// </summary>
    public enum Elm2ButtonSet
    {
        /// <summary>
        /// OK button
        /// </summary>
        OK = 0,

        /// <summary>
        /// Yes/No buttons
        /// </summary>
        YesNo = 1,
        
        /// <summary>
        /// Abort/Retry buttons
        /// </summary>
        AbortRetry = 16,

        /// <summary>
        /// Abort/Retry/Fail buttons
        /// </summary>
        AbortRetryFail = 18, // Modulo 3

        /// <summary>
        /// Abort/Retry/Ignore buttons
        /// </summary>
        AbortRetryIgnore = 20, // Modulo 5

        /// <summary>
        /// Retry/Fail buttons
        /// </summary>
        RetryFail = 22, // Modulo 11

        /// <summary>
        /// Retry/Fail/Ignore buttons
        /// </summary>
        RetryFailIgnore = 24, // Modulo 12 

        /// <summary>
        /// Exit button
        /// </summary>
        Exit = 128,

        /// <summary>
        /// Retry/Exit buttons
        /// </summary>
        RetryExit = 144, // Moulo 12

        /// <summary>
        /// Abort/Retry/Exit buttons
        /// </summary>
        AbortRetryExit = 160, // Modulo 40

        /// <summary>
        /// Abort/Exit buttons
        /// </summary>
        AbortExit = 176 // Modulo 88



    }
}
