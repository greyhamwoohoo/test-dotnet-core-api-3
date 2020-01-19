using System;

namespace GreyhamWooHoo.Interceptor.Core.Contracts
{
    public interface IAfterExecutionRule
    {
        Action<IAfterExecutionResult> Callback { get; set; }
        string MethodName { get; set; }

        IAfterExecutionRule Copy();
    }
}
