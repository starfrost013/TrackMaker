using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Core
{

    /// <summary>
    /// Contains settings for a temporary file.
    /// </summary>
    public class TemporaryFileSettings
    {
        
        /// <summary>
        /// Internal: Full path to this file. 
        /// </summary>
        internal string FullPath { get; set; }

        /// <summary>
        /// The name of this temporary file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Delete this file at startup and shutdown?
        /// </summary>
        public bool Persistent { get; set; }
       
        /// <summary>
        /// Location to save this temporary file
        /// </summary>
        public string TemporaryFileLocation { get; set; }


        public TemporaryFileSettings()
        {
            TemporaryFileLocation = $@"{System.IO.Path.GetTempPath()}\Iris"; 
        }
    }
}
