using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yeha.AcceptanceTests.Infrastructure;

namespace Yeha.AcceptanceTests
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
