using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TrackMaker.Core.Graphing
{
    /// <summary>
    /// Graphing Subsystem
    /// 
    /// Version 2.1.0
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
