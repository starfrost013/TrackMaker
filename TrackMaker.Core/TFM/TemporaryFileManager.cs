using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Core
{
    public class TemporaryFileManager
    {

        public List<TemporaryFile> TemporaryFiles { get; set; }

        public TemporaryFileManager()
        {
            TemporaryFiles = new List<TemporaryFile>(); 
        }

        public void ClearAllFiles()
        {
            foreach (TemporaryFile TF in TemporaryFiles)
            {
                if (File.Exists(TF.FullPath))
                {
                    File.Delete(TF.FullPath);
                }
                
            }
        }
    }
}
