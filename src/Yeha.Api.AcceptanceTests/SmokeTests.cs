using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yeha.Api.AcceptanceTests.Infrastructure;

namespace Yeha.Api.AcceptanceTests
{
    [TestClass]
    public class SmokeTests : AcceptanceTestBase
    {
        [TestMethod]
        public void GetPrimitivies()
        {
            var testSettings = Resolve<TestSettings>();
        }
    }
}
