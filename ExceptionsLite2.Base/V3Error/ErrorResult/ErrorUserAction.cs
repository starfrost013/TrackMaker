using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionsLite2.Base
{
    public enum ErrorUserAction
    {
        /// <summary>
        /// OK button pressed
        /// </summary>
        OK = 0,

        /// <summary>
        /// Cancel button pressed
        /// </summary>
        Cancel = 1,

        /// <summary>
        /// Yes button pressed
        /// </summary>
        Yes = 2,
        
        /// <summary>
        /// No button pressed
        /// </summary>
        No = 3,

        /// <summary>
        /// Abort button pressed
        /// </summary>
        Abort = 4,

        /// <summary>
        /// Retry button pressed
        /// </summary>
        Retry = 5,

        /// <summary>
        /// Fail button pressed
        /// </summary>
        Fail = 6,

        /// <summary>
        /// Ignore button pressed
        /// </summary>
        Ignore = 7,
        
        /// <summary>
        /// Application exited
        /// </summary>
        Exit = 8,

        /// <summary>
        /// Error Report sent and application exited
        /// </summary>
        ExitErrReport = 9
    }
}
