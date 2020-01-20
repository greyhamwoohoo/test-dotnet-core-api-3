using System;
using System.Collections.Generic;
using System.Text;

namespace GreyhamWooHoo.Interceptor.Core.Contracts
{
    public interface IBeforeExecutionResult
    {
        IBeforeExecutionRule Rule { get; }

        object[] Args { get; }
    }
}
