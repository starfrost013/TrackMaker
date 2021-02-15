using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TrackMaker.Core.Graphing
{

    /// <summary>
    /// 2020-01-25      Iris v694
    /// 
    /// StormGraph (Implements interface IGraph)
    /// 
    /// Defines a class used for graphing the performance of the Track Maker
    /// </summary>
    public class PerformanceGraph : IGraph
    {
        public GraphSettings Settings { get; set; }

        public List<GraphLine> Lines { get; set; }

        public bool Plot()
        {
            throw new NotImplementedException();
        }

        public GraphLine GetLineWithName(string KeyName)
        {
            throw new NotImplementedException();
        }

        public GraphLine AddLine(string KeyName, int StrokeThickness, Color Colour)
        {
            throw new NotImplementedException();
        }

        public void AddLine(GraphLine Ln)
        {
            if (Ln == null)
            {
                // change to error?
                Error.Throw("Error", "GS: Error: Attempted to add invalid line to graph!", ErrorSeverity.Error, 421);
                return;
            }
            else
            {
                Lines.Add(Ln);
                return;
            }
        }

        public void DeleteLineWithId(int Id)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineWithName(string Name)
        {
            throw new NotImplementedException();
        }

    }
}
