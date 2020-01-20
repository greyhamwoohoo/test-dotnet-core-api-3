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

        private readonly List<IBeforeExecutionRule> BeforeExecutionRules;
        private readonly List<IAfterExecutionRule> AfterExecutionRules;

        public InterceptorProxyBuilder()
        {
            BeforeExecutionRules = new List<IBeforeExecutionRule>();
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

        public InterceptorProxyBuilder<T> InterceptAfterExecutionOf(string theMethodCalled, Action<IAfterExecutionResult> andCallbackWith)
        {
            AfterExecutionRules.Add(new AfterExecutionRule(theMethodCalled, andCallbackWith));
            return this;
        }

        public InterceptorProxyBuilder<T> InterceptBeforeExecutionOf(string theMethodNamed, Action<IBeforeExecutionResult> andCalledbackWith)
        {
            BeforeExecutionRules.Add(new BeforeExecutionRule(theMethodNamed, andCalledbackWith));
            return this;
        }

        public T Build()
        {
            if (null == _instance) throw new InvalidOperationException($"You must call the {nameof(For)} method and pass in a concrete instance of the interface implementation. ");

            return InterceptorProxy<T>.Create(_instance, BeforeExecutionRules, AfterExecutionRules, _taskWaiter);
        }
    }
}
