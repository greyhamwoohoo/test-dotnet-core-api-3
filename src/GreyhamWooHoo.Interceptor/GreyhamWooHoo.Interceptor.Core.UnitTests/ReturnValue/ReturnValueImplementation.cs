namespace GreyhamWooHoo.Interceptor.Core.UnitTests.ReturnValue
{
    public class ReturnValueImplementation : IReturnValueInterface
    {
        public string Message { get; set; }

        public void TheVoidMethod()
        {
            Message = $"Invoked: {nameof(TheVoidMethod)}";
        }
    }
}
