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
        public TemporaryFileSettings Settings { get; set; }
        /// <summary>
        /// The stream used for read/write this file. 
        /// </summary>
        public FileStream Stream { get; set; }

        public TemporaryFile()
        {
            Settings = new TemporaryFileSettings();

            Random Rnd = new Random();
            int XID = Rnd.Next(100000, 999999);

            Settings.Name = $"Iris{XID}.tmp";
        }

        public FileStream Create()
        {
            string FullPath = $@"{Settings.TemporaryFileLocation}\{Settings.Name}";
            Stream = new FileStream(FullPath, FileMode.OpenOrCreate);
            return Stream;
            
        }
       

    }
}
