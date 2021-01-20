using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Core
{

    /// <summary>
    /// Temporary File Manager API v1.2.0
    /// 
    /// 2021-01-18
    /// </summary>
    public class TemporaryFileManager
    {
        public static int TFMAPI_VersionMajor = 1;
        public static int TFMAPI_VersionMinor = 2;
        public static int TFMAPI_VersionRevision = 0;

        public List<TemporaryFile> TemporaryFiles { get; set; }

        public TemporaryFileManager()
        {
            TemporaryFiles = new List<TemporaryFile>(); 
        }

        /// <summary>
        /// Create a new temporary file with optional name and settings. Returns a FileStream that can be used for opening things.
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="TFS"></param>
        /// <returns></returns>
        public FileStream CreateNewFile(TemporaryFileSettings TFS = null)
        {
            TemporaryFile TF = new TemporaryFile();

            if (TFS != null)
            {
                TF.Settings = TFS;
                
            }

            return TF.Create(); 

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

        public FileStream OpenTemporaryFileWithName(string Name)
        {
            foreach (TemporaryFile TF in TemporaryFiles)
            {
                if (TF.Settings.Name == Name)
                {
                    try
                    {
                        TF.Stream = new FileStream(TF.Settings.FullPath, FileMode.Open);
                        return TF.Stream;
                    }
                    catch (FileNotFoundException)
                    {
                        return null; 
                    }

                }
            }

            return null; 
        }
    }
}
