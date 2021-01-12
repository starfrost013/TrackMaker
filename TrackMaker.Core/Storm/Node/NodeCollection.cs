using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TrackMaker.Core
{
    /// <summary>
    /// Iris       v2.1.654
    /// 
    /// Node collection 
    /// </summary>
    public class NodeCollection
    {
        private List<Node> Nodes { get; set; }

        public void Add(Node NodeObject) => Nodes.Add(NodeObject);
        public void Remove(Node NodeObject) => Nodes.Remove(NodeObject);

        public NodeCollection()
        {
            Nodes = new List<Node>();
        }
    }
}
