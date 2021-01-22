using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Core
{

    /// <summary>
    /// Temporary File Manager API v1.3.0
    /// 
    /// 2021-01-21
    /// 
    /// v1.3.0      2021-01-21  Iris v683: Add DelayClearUntilNextExit to allow most recent log to be preserved 
    /// v1.2.0      2021-01-18  Rename many APIs to have better names
    /// </summary>
    public class TemporaryFileManager
    {
        public static int TFMAPI_VersionMajor = 1;
        public static int TFMAPI_VersionMinor = 3;
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
        public FileStream CreateNewFile(TemporaryFileApplicationSettings TFS = null)
        {
            TemporaryFile TF = new TemporaryFile();

            if (TFS != null)
            {
                TF.ApplicationSettings = TFS;
                
            }

            return TF.Create(); 

        }

        public TemporaryFile CreateNewEmptyFile() => new TemporaryFile();

        /// <summary>
        /// Clear all temporary files.
        /// 
        /// If IsShutdown is true, will not delete files that have the DelayClearUntilNextStart property.
        /// </summary>
        /// <param name="IsShutdown"></param>
        public void ClearAllFiles(bool IsShutdown = false)
        {
            foreach (TemporaryFile TF in TemporaryFiles)
            {
                if (File.Exists(TF.FullPath))
                {
                    if (!TF.ApplicationSettings.Persistent)
                    {
                        if (TF.ApplicationSettings.DelayClearUntilNextStart && !IsShutdown)
                        {
                            File.Delete(TF.FullPath);
                        }
                        else
                        {
                            continue; 
                        }
                    }

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
                if (TF.ApplicationSettings.Name == Name) return TF; 
            }

            return null; 
        }

        public FileStream OpenTemporaryFileWithName(string Name)
        {
            foreach (TemporaryFile TF in TemporaryFiles)
            {
                if (TF.ApplicationSettings.Name == Name)
                {
                    try
                    {
                        TF.Stream = new FileStream(TF.ApplicationSettings.FullPath, FileMode.Open);
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
