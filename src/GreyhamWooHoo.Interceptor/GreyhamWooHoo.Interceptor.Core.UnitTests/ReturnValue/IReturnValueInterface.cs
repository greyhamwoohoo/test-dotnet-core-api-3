using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.ReturnValue
{
    interface IReturnValueInterface
    {
        void TheVoidMethod();
        int TheIntMethod();
        Task TheTaskVoidMethod();
        Task<int> TheTaskIntMethod();
        Task TheExceptionTaskMethod();
    }
}
