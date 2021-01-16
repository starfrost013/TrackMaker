using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMaker.Core;

namespace Track_Maker
{
    /// <summary>
    /// Result class for importing nodes
    /// </summary>
    public class NodeImportResult
    {
        public List<Node> Nodes { get; set; }
        public bool Empty { get; set; }
        public bool Successful { get; set; }

        public NodeImportResult()
        {
            Nodes = new List<Node>(); 
        }
    }
}
