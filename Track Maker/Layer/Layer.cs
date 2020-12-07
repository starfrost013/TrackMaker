using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    public class Layer
    {
        public List<Storm> AssociatedStorms { get; set; }
        public Storm CurrentStorm { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public Guid LayerId { get; set; }
        public int ZIndex { get; set; } // The ordering of this layer
        
       
        public Layer()
        {
            AssociatedStorms = new List<Storm>();
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
