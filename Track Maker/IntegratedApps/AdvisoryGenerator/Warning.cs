using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvisoryGenerator
{
    
    public class Warning
    {
        public string Type { get; set; }
        public string IssuingAgency { get; set; }
        public string AreaFrom { get; set; }
        public string AreaTo { get; set; }
        public string Text { get; set; }
    }
}
