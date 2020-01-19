using GreyhamWooHoo.Interceptor.Core.Contracts;

namespace GreyhamWooHoo.Interceptor.Core.Results
{
    public class CaptureReturnValueResult : ICaptureReturnValueResult
    {
        public CaptureReturnValueResult(ICaptureReturnValueRule rule) : this(rule, false, null)
        {
        }

        public CaptureReturnValueResult(ICaptureReturnValueRule rule, bool hasReturnValue, object returnValue)
        {
            Rule = rule;
            HasReturnValue = hasReturnValue;
            ReturnValue = returnValue;
        }

        public ICaptureReturnValueRule Rule { get; }

        public bool HasReturnValue { get; }

        public object ReturnValue { get; }
    }
}
