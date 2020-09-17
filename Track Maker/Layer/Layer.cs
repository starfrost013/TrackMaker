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
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public Guid LayerId { get; set; }
       
        public Layer()
        {
            AssociatedStorms = new List<Storm>();
            GenerateGUID();
        }

        private void GenerateGUID()
        {
            LayerId = Guid.NewGuid();
        }

        public void AddStorm(Storm Sto, bool MakeCurrent = false)
        {
            AssociatedStorms.Add(Sto);

            if (MakeCurrent) CurrentStorm = Sto; 
        }

        public void RemoveStorm(Storm Sto) => AssociatedStorms.Remove(Sto);

        public void ClearStorms() => AssociatedStorms.Clear();

    }
}
