using System;

namespace GreyhamWooHoo.Interceptor.Core.Contracts
{
    public interface ICaptureReturnValueRule
    {
        Action<ICaptureReturnValueResult> Callback { get; set; }
        string MethodName { get; set; }

        ICaptureReturnValueRule Copy();
    }
}
