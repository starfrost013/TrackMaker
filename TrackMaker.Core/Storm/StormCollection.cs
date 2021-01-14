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
    /// Holds a collection of storms. 
    /// </summary>
    public class StormCollection
    {

        /// <summary>
        /// IrisAPI
        /// 
        /// v2.1.654
        /// 
        /// Storm list is private; all interaction with it is done throuh the various class APIs 
        /// </summary>
        [XmlElement("Storm")]
        private List<Storm> Storms { get; set; }

        public StormCollection()
        {
            Storms = new List<Storm>();
        }

        /// <summary>
        /// Add a Storm to this storm collection. 
        /// </summary>
        /// <param name="StormObject">The Storm object to add.</param>
        public void Add(Storm StormObject) => Storms.Add(StormObject);

        /// <summary>
        /// Remove a Storm from this storm collection. 
        /// </summary>
        /// <param name="StormObject">The storm object to remove.</param>
        public void Remove(Storm StormObject) => Storms.Remove(StormObject);

        public void Clear() => Storms.Clear(); 
    }
}
