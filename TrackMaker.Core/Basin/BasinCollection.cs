using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;


namespace TrackMaker.Core
{
    
    /// <summary>
    /// Basin class for XML Serialisation
    /// 
    /// 2021-01-11  v2.1.654
    /// </summary>
    /// 
    [XmlRoot("Basins")]
    public class BasinCollection : IEnumerable
    {

        /// <summary>
        /// IrisAPI
        /// 
        /// V2.1.654
        /// 
        /// List is private.
        /// </summary>
        [XmlElement("Basin")]
        private List<Basin> Basins { get; set; }

        public BasinCollection()
        {
            Basins = new List<Basin>();
        }

        public void Add(Basin BasinObject) => Basins.Add(BasinObject);
        public void Remove(Basin BasinObject) => Basins.Remove(BasinObject);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public BasinEnumerator GetEnumerator() => new BasinEnumerator(); 
    }

    public class BasinEnumerator : IEnumerator
    {
        public Basin[] BasinCollection { get; set; }
        public object Current { get; set; }
        public int Position { get; set; }

        /// <summary>
        /// IrisEnumerator: Move next
        /// </summary>
        public bool MoveNext()
        {
            Position++;
            Current = BasinCollection;
            return (Position < BasinCollection.Length);
        }

        public void Reset() => throw new NotImplementedException("Never call IrisEnumerator.Reset()!"); 

    }
}
