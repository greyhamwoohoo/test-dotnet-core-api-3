using GreyhamWooHoo.Interceptor.Core.Contracts;
using System;

namespace GreyhamWooHoo.Interceptor.Core.Results
{
    public class BeforeExectionResult : IBeforeExecutionResult
    {
        public BeforeExectionResult(IBeforeExecutionRule rule)
        {
            Rule = rule ?? throw new System.ArgumentNullException(nameof(rule));
        }

        public BeforeExectionResult(IBeforeExecutionRule rule, object[] args)
        {
            Rule = rule ?? throw new ArgumentNullException(nameof(rule));
            Args = args ?? throw new ArgumentNullException(nameof(args));
        }

        public IBeforeExecutionRule Rule { get; }

        public object[] Args { get; }
    }
}
