using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection; 
using System.Text;
using System.Threading.Tasks;

namespace Track_Maker.V3Error
{
    /// <summary>
    /// Priscilla+ (v2.1) / Dano (v3.0) centralised error service
    /// 
    /// Primarily for localisation purposes
    /// </summary>
    /// 

    public delegate ErrorResult HandleError();
    public class V3Error
    {
        public Exception Exception { get; set; }
        /// <summary>
        /// Error ID
        /// </summary>
        public int ErrorId { get; set; }
        public string ErrorName { get; set; }
        public string ErrorMessage { get; set; }
        public Delegate ResultHandler { get; set; }
        public bool UsesCustomErrorHandler { get; set; }

        public ErrorResult Throw()
        {
            if (UsesCustomErrorHandler)
            {
                Delegate Dgt = new HandleError(Throw);
                

            }
            MethodInfo MI = GenerateMethodName();
            //MI.Invoke...
            return new ErrorResult();
        }

        private MethodInfo GenerateMethodName()
        {
            try
            {
                Type StaticErrHandlerMethodType = typeof(ErrorHandlers);

                string MethodName = $"ErrorHandler_ErrID{ErrorId}";

                MethodInfo MI = StaticErrHandlerMethodType.GetMethod(MethodName);

                return MI;
            }
            catch (AmbiguousMatchException)
            {
                throw new Exception("ELM2: Cannot find error handler method!"); 
            }
            catch (ArgumentNullException)
            {
                throw new Exception("ELM2: Cannot acquire nonexistent error handler method!");
            }
                

        }
    
    }
}
