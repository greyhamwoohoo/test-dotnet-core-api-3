using GreyhamWooHoo.Interceptor.Core.Contracts;
using System;

namespace GreyhamWooHoo.Interceptor.Core.Rules
{
    public class AfterExecutionRule : IAfterExecutionRule
    {
        public AfterExecutionRule(string methodName, Action<IAfterExecutionResult> callback)
        {
            MethodName = methodName;
            Callback = callback;
        }

        public string MethodName { get; set; }
        public Action<IAfterExecutionResult> Callback { get; set; }

        public IAfterExecutionRule Copy()
        {
            return new AfterExecutionRule(MethodName, Callback);
        }
    }
}
