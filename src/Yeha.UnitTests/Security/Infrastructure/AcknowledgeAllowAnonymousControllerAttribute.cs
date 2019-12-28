using System;

namespace Yeha.UnitTests.Security.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AcknowledgeAllowAnonymousControllerAttribute : Attribute
    {
        public AcknowledgeAllowAnonymousControllerAttribute()
        {
        }

        public Type Controller { get; set; }
    }
}
