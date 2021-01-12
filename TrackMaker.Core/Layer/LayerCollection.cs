using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization; 
using System.Threading.Tasks;

namespace TrackMaker.Core
{
    public class LayerCollection
    {
        [XmlElement("Layer")]
        private List<Layer> Layers { get; set; }

        public void Add(Layer BasinObject) => Layers.Add(BasinObject);
        public void Remove(Layer BasinObject) => Layers.Remove(BasinObject);

        public LayerCollection()
        {
            Layers = new List<Layer>(); 
        }
    }
}
