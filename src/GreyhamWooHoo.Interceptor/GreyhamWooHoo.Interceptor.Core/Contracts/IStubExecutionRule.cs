namespace GreyhamWooHoo.Interceptor.Core.Contracts
{
    public interface IStubExecutionRule : IExecutionRule
    {
        public object Value { get; }
    }
}
