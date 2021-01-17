using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Track_Maker.Graphing
{
    /// <summary>
    /// Dano (3.0) graphing mode individual graph
    /// </summary>
    public interface IGraph
    {
        string Name { get; set; }
        List<GraphLine> Lines { get; set; }
        bool Plot();
        GraphLine GetLineWithName();
        void AddLine(string Name, Color LineColour);
        void DeleteLineWithName(string Name);
        void DeleteLineWithId(int Id); 
    }
}
