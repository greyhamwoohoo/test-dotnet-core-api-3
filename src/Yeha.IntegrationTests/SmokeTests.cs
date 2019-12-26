using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Yeha.Models;

namespace Yeha.IntegrationTests
{
    [TestClass]
    public class SmokeTests : IntegrationTestBase
    {
        [TestMethod]
        [TestCategory("Smoke")]
        public async Task WhenIGetPrimitivies_PrimitiviesAreReturned()
        {
            // Act
            var response = await TestClient.GetAsync("/api/primitives");
            response.EnsureSuccessStatusCode();
        }
    }
}
