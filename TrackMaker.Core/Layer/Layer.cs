using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace TrackMaker.Core
{
    public class Layer
    {
        // for now

        /// <summary>
        /// The list of storms associated with this layer
        /// </summary>
        /// 
        [XmlElement("Storms")]; 
        public StormCollection AssociatedStorms { get; set; }
        public Storm CurrentStorm { get; set; }
        public bool Enabled { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Id")]
        public Guid LayerId { get; set; }
        public int ZIndex { get; set; } // The ordering of this layer
        
       
        public Layer()
        {
            AssociatedStorms = new StormCollection(); 
            Enabled = true;
            GenerateGUID();
        }

        private void GenerateGUID() => LayerId = Guid.NewGuid(); 

        public void AddStorm(Storm Sto, bool MakeCurrent = false)
        {
            AssociatedStorms.Add(Sto);

            if (MakeCurrent) CurrentStorm = Sto; 
        }

        public void RemoveStorm(Storm Sto) => AssociatedStorms.Remove(Sto);

        public Storm GetCurrentStorm()
        {
            if (CurrentStorm != null)
            {
                return CurrentStorm;
            }
            else
            {
                Error.Throw("E202", "Attempted to get current storm when there are no storms on this layer!", ErrorSeverity.Error, 202);
                return null; 
            }
        }

        public void ClearStorms() => AssociatedStorms.Clear();

        public void RenameLayer(string Nm) => Name = Nm; 
    }
}
