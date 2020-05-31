using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Track_Maker
{
    public class Node
    {
        public Category Category { get; set; } // New for Dano. The category of this node.
        public int Id { get; set; } // The id of the node
        public int Intensity { get; set; } // intensity mph
        public StormType NodeType { get; set; } // as any node can be any type.
        public Point Position { get; set; } // the position
    }
}
