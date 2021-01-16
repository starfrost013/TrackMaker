using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Util.InputValidation
{
    public class ValidationResult
    {
        public bool Successful { get; set; }
        public object ValidationObject { get; set; }
    }
}
