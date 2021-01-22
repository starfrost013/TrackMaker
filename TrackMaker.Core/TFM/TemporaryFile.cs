using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Core
{
    /// <summary>
    /// Iris        v2.1.675 (2020-01-17):
    /// 
    /// Temporary File Management
    /// 
    /// TemporaryFile Class: Holds information about temporary files.
    /// </summary>
    public class TemporaryFile
    {

        /// <summary>
        /// The full path to this file. 
        /// </summary>
        internal string FullPath { get; set; }

        /// <summary>
        /// The settings used for saving this temporary file.
        /// </summary>
        public TemporaryFileApplicationSettings ApplicationSettings { get; set; }
        /// <summary>
        /// The stream used for read/write this file. 
        /// </summary>
        public FileStream Stream { get; set; }

        public TemporaryFile()
        {
            ApplicationSettings = new TemporaryFileApplicationSettings();

            Random Rnd = new Random();
            int XID = Rnd.Next(100000, 999999);

            ApplicationSettings.Name = $"Iris{XID}.tmp";
        }

        public FileStream Create()
        {
            string FullPath = $@"{ApplicationSettings.TemporaryFileLocation}\{ApplicationSettings.Name}";
            Stream = new FileStream(FullPath, FileMode.OpenOrCreate);
            return Stream;
            
        }
       

    }
}
