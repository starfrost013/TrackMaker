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

        public FileStream CreateNewFile(string FileName = null, TemporaryFileSettings TFS = null )
        {
            TemporaryFile TF = new TemporaryFile();

            if (FileName == null)
            {
                return TF.Create();
            }
            else
            {
                if (TFS != null)
                {
                    TF.Settings = TFS;
                }
                else
                {
                    TF.Settings.Name = FileName;
                }

                return TF.Create();
            }
        }

        public TemporaryFile CreateNewEmptyFile() => new TemporaryFile();

        public void ClearAllFiles()
        {
            foreach (TemporaryFile TF in TemporaryFiles)
            {
                if (File.Exists(TF.FullPath))
                {
                    File.Delete(TF.FullPath);
                    
                }
                else
                {
                    continue; // do something later.
                }
            }

            TemporaryFiles.Clear();
            return;
        }

        public TemporaryFile GetTemporaryFileWithName(string Name)
        {
            foreach (TemporaryFile TF in TemporaryFiles)
            {
                if (TF.Settings.Name == Name) return TF; 
            }

            return null; 
        }
    }
}
