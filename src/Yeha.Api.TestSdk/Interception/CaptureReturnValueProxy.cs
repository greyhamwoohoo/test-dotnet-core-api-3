using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Yeha.Api.TestSdk.Interception
{
    /// <summary>
    /// Capture the ReturnValue of any methods
    /// </summary>
    /// <remarks>
    /// While the following reference is for Aspect Oriented Programming (static/source-code-level attributes), I have used some of the infrastructure here:
    /// Reference: https://www.c-sharpcorner.com/article/aspect-oriented-programming-in-c-sharp-using-dispatchproxy/
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class CaptureReturnValueProxy<T> : DispatchProxy
    {
        private T _originalImplementation;
        private IEnumerable<CaptureReturnValueRule> _interceptors;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var name = targetMethod.Name;

            var result = targetMethod.Invoke(_originalImplementation, args);

            var resultTask = result as Task;
            if (resultTask != null)
            {
                throw new System.InvalidOperationException($"The return type is a Task - to implement the handling for this, see: https://www.c-sharpcorner.com/article/aspect-oriented-programming-in-c-sharp-using-dispatchproxy/");
            }

            var interceptedMethod = _interceptors.FirstOrDefault(f => f.MethodName == name);
            if (interceptedMethod != null)
            {
                // We now callout with the original value: it is better the recipient serialize / deserialize this by casting to the expected type first. 
                interceptedMethod.Callback(result, interceptedMethod);
            }

            return result;
        }

        private void SetParameters(T originalImplementation, IEnumerable<CaptureReturnValueRule> interceptors)
        {
            _originalImplementation = originalImplementation;
            _interceptors = interceptors.Select(i => new CaptureReturnValueRule(i.MethodName, i.Callback));
        }

        public static T Create(T originalImplementation, IEnumerable<CaptureReturnValueRule> interceptors)
        {
            object proxy = Create<T, CaptureReturnValueProxy<T>>();

            ((CaptureReturnValueProxy<T>)proxy).SetParameters(originalImplementation, interceptors);

            return (T)proxy;
        }
    }
}
