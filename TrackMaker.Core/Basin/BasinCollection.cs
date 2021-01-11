using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace TrackMaker.Core.Basin
{
    
    /// <summary>
    /// Root class for XML Serialisation
    /// 
    /// 2021-01-11  v2.1.654
    /// </summary>
    /// 
    [XmlRoot("Basins")]
    public class BasinCollection
    {
        [XmlElement("Basin")]
        public List<Basin> Basins { get; set; }

        public BasinCollection()
        {
            Basins = new List<Basin>();
        }
    }
}
