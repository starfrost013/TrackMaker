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
    /// Storm Type class for XML Deserialisation
    /// 
    /// 2021-01-13  v2.1.658
    /// </summary>
    [XmlRoot("StormTypes")]
    public class StormTypeCollection
    {
        [XmlElement("StormType")]
        private List<StormType2> StormTypes { get; set; }
    }
}
