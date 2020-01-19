using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.Rules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.Builders
{
    public class InterceptorProxyBuilder<T>
    {
        private T _instance;
        private Action<Task> _taskWaiter;

        private readonly List<IAfterExecutionRule> AfterExecutionRules;

        public InterceptorProxyBuilder()
        {
            AfterExecutionRules = new List<IAfterExecutionRule>();
            _taskWaiter = task => task.Wait();
        }

        public InterceptorProxyBuilder<T> For(T instance)
        {
            _instance = instance;
            return this;
        }

        public InterceptorProxyBuilder<T> WithTaskAwaiter(Action<Task> taskWaiter)
        {
            _taskWaiter = taskWaiter;
            return this;
        }

        public InterceptorProxyBuilder<T> InterceptReturnValueOf(string theMethodCalled, Action<IAfterExecutionResult> andCallback)
        {
            AfterExecutionRules.Add(new AfterExecutionRule(theMethodCalled, andCallback));
            return this;
        }

        public T Build()
        {
            if (null == _instance) throw new InvalidOperationException($"You must call the {nameof(For)} method and pass in a concrete instance of the interface implementation. ");

            return InterceptorProxy<T>.Create(_instance, AfterExecutionRules, _taskWaiter);
        }
    }
}
