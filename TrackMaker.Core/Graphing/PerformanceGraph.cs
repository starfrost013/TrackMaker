using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TrackMaker.Core
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

        public GraphLine GetLineWithName()
        {
            throw new NotImplementedException();
        }

        public GraphLine AddLine(string String, Color Colour)
        {
            throw new NotImplementedException();
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
