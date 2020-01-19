using GreyhamWooHoo.Interceptor.Core.Contracts;
using System;

namespace GreyhamWooHoo.Interceptor.Core.Rules
{
    public class CaptureReturnValueRule : ICaptureReturnValueRule
    {
        public CaptureReturnValueRule(string methodName, Action<ICaptureReturnValueResult> callback)
        {
            MethodName = methodName;
            Callback = callback;
        }

        public string MethodName { get; set; }
        public Action<ICaptureReturnValueResult> Callback { get; set; }

        public ICaptureReturnValueRule Copy()
        {
            return new CaptureReturnValueRule(MethodName, Callback);
        }
    }
}
