using System;
using System.Collections.Generic;
using System.Text;

namespace Yeha.Api.TestSdk.Interception
{
    public interface ICaptureReturnValueRule
    {
        Action<object, ICaptureReturnValueRule> Callback { get; set; }
        string MethodName { get; set; }

        CaptureReturnValueRule Copy();
    }
}
