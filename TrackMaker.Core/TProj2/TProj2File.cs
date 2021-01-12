using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization; 
using System.Threading.Tasks;

namespace TrackMaker.Core
{
    /// <summary>
    /// Iris        v2.1.654
    /// 
    /// TProj2 File class (refactor)
    /// </summary>
    /// 
    [XmlRoot("Project")]
    public class TProj2File
    {
        public TProj2Metadata Metadata { get; set; }
        //public Project TProject { get; set; }
    }
}
