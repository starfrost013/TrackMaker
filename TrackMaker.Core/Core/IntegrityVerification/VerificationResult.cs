using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Core
{
    public class VerificationResult
    {
        public CriticalFile File { get; set; }
        public string FailureReason { get; set; }
        public bool WasFileValid { get; set; }
        public bool Successful { get; set; }
    }
}
