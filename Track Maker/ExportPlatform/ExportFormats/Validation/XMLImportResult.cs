using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMaker.Core; 

namespace Track_Maker
{

    /// <summary>
    /// ExportXMLv2 Import Result class
    /// 
    /// 2020-09-18, modified 2020-09-19
    /// 
    /// Version 1.1
    /// 
    /// DEPRECATED 2020-12-20: generic ImportResult class introduced, will be removed in future build
    /// </summary>
    public class XMLImportResult__DEPRECATED
    {
        public bool Cancelled { get; set; }
        public bool Successful { get; set; }
        public Project Project { get; set; }
    }
}
