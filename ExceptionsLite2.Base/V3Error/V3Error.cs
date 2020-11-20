using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection; 
using System.Text;
using System.Threading.Tasks;

namespace ExceptionsLite2.Base
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
        public List<string> CustomErrorHandlerParameters { get; set; }
        public virtual ErrorResult Throw()
        {
            try
            {
                ErrorResult Result = new ErrorResult();
                
                if (UsesCustomErrorHandler)
                {
                    Delegate Dgt = new HandleError(Throw);
                    MethodInfo MI = GenerateMethodName();

                    // ErrorHandlers is static, so we use null
                    object MthdResult = MI.Invoke(null, CustomErrorHandlerParameters.ToArray());

                    Result = (ErrorResult)MthdResult;
                }
                else
                {
                    // default error handler if an Implementation DLL has not implemented(?) it

                   
                }

                return Result;
            }
            catch (TargetException err)
            {
#if DEBUG
                throw new Exception($"An error occurred while attempting to handle an error.\n\nTargetException (attempted to invoke nonexistent class!): \n\n{err}");
#else
                throw new Exception("An error occurred while attempting to handle an error.\n\nTargetException:");
#endif
            }
            catch (TargetInvocationException err)
            {
#if DEBUG
                throw new Exception($"An error occurred while attempting to handle an error.\n\nTargetInvocationException: \n\n{err}");
#else
                throw new Exception("An error occurred while attempting to handle an error.\n\nTargetInvocationException:");
#endif
            }
            catch (MethodAccessException err)
            {
#if DEBUG
                throw new Exception($"An error occurred while attempting to handle an error.\n\nMethodAccessException (Protection level mismatch!): \n\n{err}");
#else
                throw new Exception("An error occurred while attempting to handle an error.\n\nMethodAccessException:");
#endif
            }
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
