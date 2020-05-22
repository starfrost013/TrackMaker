using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker
{
    public class Project
    {
        public List<Basin> Basins { get; set; }
        public Basin CurrentBasin { get; set; }
        public string Name { get; set; }
        
    }
}
