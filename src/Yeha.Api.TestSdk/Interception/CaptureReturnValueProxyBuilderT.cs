using System;
using System.Collections.Generic;
using System.Text;

namespace Yeha.Api.TestSdk.Interception
{
    public class CaptureReturnValueProxyBuilder<T>
    {
        private T _instance;

        private readonly List<CaptureReturnValueRule> CaptureReturnValueRules;

        public CaptureReturnValueProxyBuilder()
        {
            CaptureReturnValueRules = new List<CaptureReturnValueRule>();
        }

        public CaptureReturnValueProxyBuilder<T> For(T instance)
        {
            _instance = instance;
            return this;
        }

        public CaptureReturnValueProxyBuilder<T> InterceptReturnValueOf(string theMethodCalled, Action<object, ICaptureReturnValueRule> andCallback)
        {
            CaptureReturnValueRules.Add(new CaptureReturnValueRule(theMethodCalled, andCallback));
            return this;
        }

        public T Build()
        {
            if (null == _instance) throw new System.InvalidOperationException($"You must call the {nameof(For)} method and pass in a concrete instance of the interface implementation. ");

            return CaptureReturnValueProxy<T>.Create(_instance, CaptureReturnValueRules);
        }
    }
}
