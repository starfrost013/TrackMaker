using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Track_Maker
{

    /// <summary>
    /// A track node.
    /// 
    /// A particular point on a track. It may have a storm type, a position, and an intensity. At some point in the future it will store wind radii etc.
    /// </summary>
    public class Node
    {
        public int Id { get; set; } // The id of the node
        public int Intensity { get; set; } // intensity mph
        public Point Position { get; set; } // the position
        public StormType NodeType { get; set; } // as any node can be any type.
    }
}
