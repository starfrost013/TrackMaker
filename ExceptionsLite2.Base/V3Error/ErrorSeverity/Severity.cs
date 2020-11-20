using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionsLite2.Base
{
    /// <summary>
    /// ErrorSeverity
    /// </summary>
    public enum ErrorSeverity
    {
        /// <summary>
        /// Message / notification - no error has occurred
        /// </summary>
        Message = 0,

        /// <summary>
        /// A warning - "doing this may do something bad" et al
        /// </summary>
        Warning = 1,
        
        /// <summary>
        /// Non-fatal error
        /// </summary>
        Error = 2,

        /// <summary>
        /// Fatal error
        /// </summary>
        Fatal = 3
    }
}
