using System;

namespace Yeha.UnitTests.Security.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AcknowledgeAllowAnonymousMethodAttribute : Attribute
    {
        public AcknowledgeAllowAnonymousMethodAttribute()
        {
        }

        public Type Controller { get; set; }
        public string MethodName { get; set; }
        public string FullyQualifiedName => $"{Controller?.FullName}.{MethodName}";
    }
}
