using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfrost.UL5.InputValidation
{
    public static class ResultFactory<T> where T : GenericResult<T>
    {
        public static T GetResultClass(GenericResult<T> GenericResult)
        {
            T TypeT = (T)GenericResult;
            return TypeT;

        }
    }
}
