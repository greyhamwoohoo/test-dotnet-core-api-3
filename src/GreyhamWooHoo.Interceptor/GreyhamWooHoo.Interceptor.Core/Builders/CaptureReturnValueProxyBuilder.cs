using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.Rules;
using System;
using System.Collections.Generic;
using Yeha.Api.TestSdk.Interception;

namespace GreyhamWooHoo.Interceptor.Core.Builders
{
    public class CaptureReturnValueProxyBuilder<T>
    {
        private T _instance;

        private readonly List<ICaptureReturnValueRule> CaptureReturnValueRules;

        public CaptureReturnValueProxyBuilder()
        {
            CaptureReturnValueRules = new List<ICaptureReturnValueRule>();
        }

        public CaptureReturnValueProxyBuilder<T> For(T instance)
        {
            _instance = instance;
            return this;
        }

        public CaptureReturnValueProxyBuilder<T> InterceptReturnValueOf(string theMethodCalled, Action<ICaptureReturnValueResult> andCallback)
        {
            CaptureReturnValueRules.Add(new CaptureReturnValueRule((string)theMethodCalled, (Action<ICaptureReturnValueResult>)andCallback));
            return this;
        }

        public T Build()
        {
            if (null == _instance) throw new InvalidOperationException($"You must call the {nameof(For)} method and pass in a concrete instance of the interface implementation. ");

            return CaptureReturnValueProxy<T>.Create(_instance, CaptureReturnValueRules);
        }
    }
}
