using System;
using System.Collections.Generic;
using System.Text;

namespace Yeha.Api.TestSdk.Interception
{
    public class CaptureReturnValueRule : ICaptureReturnValueRule
    {
        public CaptureReturnValueRule(string methodName, Action<object, ICaptureReturnValueRule> callback)
        {
            MethodName = methodName;
            Callback = callback;
        }

        public string MethodName { get; set; }
        public Action<object, ICaptureReturnValueRule> Callback { get; set; }

        public CaptureReturnValueRule Copy()
        {
            return new CaptureReturnValueRule(MethodName, Callback);
        }
    }
}
