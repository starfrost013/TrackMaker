using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker.V3Error
{
    /// <summary>
    /// Priscilla+ (v2.1) / Dano (v3.0) centralised error service
    /// 
    /// Primarily for localisation purposes
    /// </summary>
    /// 

    public delegate ErrorResult HandleError(); 
    public class V3Error
    {
        public Exception Exception { get; set; }
        /// <summary>
        /// Error ID
        /// </summary>
        public int ErrorId { get; set; }
        public string ErrorName { get; set; }
        public ErrorResult ResultHandler { get; set; }
    
    }
}
