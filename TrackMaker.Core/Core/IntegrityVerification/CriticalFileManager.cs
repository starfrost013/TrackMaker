using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Core
{
    public class CriticalFileManager
    {
#if DEBUG
        public bool DeveloperMode = true;
#else
        public bool DeveloperMode = false;
#endif
        public List<CriticalFile> CriticalFiles { get; set; }

        public CriticalFileManager()
        {
            CriticalFiles = new List<CriticalFile>();
        }

        public void VerifyAll()
        {
            if (DeveloperMode) return;

            foreach (CriticalFile CF in CriticalFiles)
            {
                VerificationResult VR = CF.Verify();

                if (!VR.Successful && VR.WasFileValid)
                {
                    Error.Throw("Fatal!", $"Fatal error.\nSoftware integrity verification failure:\nAn error occurred verifying the file {VR.File.Path}: {VR.FailureReason}\n" +
                        $"Please reinstall the Track Maker or turn on DeveloperMode if you are working with the source code - this is on by default in debug builds.", ErrorSeverity.FatalError, 420);
                }
                else
                {
                    continue; 
                }
            }
        }
    }
}
