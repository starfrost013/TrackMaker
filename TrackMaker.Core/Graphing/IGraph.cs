using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TrackMaker.Core
{
    /// <summary>
    /// Dano (3.0) graphing mode individual graph
    /// 
    /// Graph Interface Version 1.2.0
    /// </summary>
    public interface IGraph
    {
        GraphSettings Settings { get; set; }
        List<GraphLine> Lines { get; set; }
        bool Plot();
        GraphLine GetLineWithName(string KeyName);
        GraphLine AddLine(string KeyName, int StrokeThickness, Color Colour);
        void DeleteLineWithName(string Name);
        void DeleteLineWithId(int Id); 
    }
}
