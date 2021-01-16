using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrackMaker.Core; 

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
        public StormType2 NodeType { get; set; } // as any node can be any type.

        public bool IsRealType()
        {
            if (NodeType == null)
            {
                Error.Throw("FATAL!!", "Attempted to call Node.IsRealType() on node with invalid NodeType", ErrorSeverity.FatalError, 247);
                return false;
            }

            // Iris 2.1 / Dano 3.0: ID system
            return (NodeType.Name == "Tropical"
                || NodeType.Name == "Subtropical"
                || NodeType.Name == "Extratropical");

        }


    }
}
