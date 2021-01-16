using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMaker.Util.InputValidation
{
    /// <summary>
    /// Generic result class
    /// </summary>
    /// <typeparam name="T"The type that one wishes to implement</typeparam>
    public class GenericResult<T> : IResult
    {
        public string ResultName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Please do not use - this is only for derivation
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        public ValidationResult Validate(object Object)
        {
            throw new NotImplementedException(); 
        }
    }
}
