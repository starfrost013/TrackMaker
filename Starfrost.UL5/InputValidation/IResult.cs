using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Util.InputValidation
{
    public interface IResult
    {
        string ResultName { get; set; }

        ValidationResult Validate(object Input); 
    }
}
