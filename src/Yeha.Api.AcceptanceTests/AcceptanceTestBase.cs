using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Yeha.Api;
using Yeha.Api.AcceptanceTests.Infrastructure;

namespace Yeha.Api.AcceptanceTests
{
    /// <summary>
    /// This implementation uses a 'hard' port binding, a real HTTP Servier and HTTP Client. 
    /// </summary>
    [TestClass]
    public class AcceptanceTestBase
    {
        public TestContext TestContext { get; set; }

        protected IHost Host => _host ?? throw new InvalidOperationException($"You must initialize the host before accessing it. ");

        private IHost _host;
        private IServiceProvider ServiceProvider => TestContext.Properties["ServiceProvider"] as IServiceProvider ?? throw new InvalidOperationException($"The container must be initialized and stored in the TestContext");

        private string _logPath;

        [TestInitialize]
        public async Task SetupAcceptanceTestBase()
        {
            var testSettings = Resolve<TestSettings>();

            _logPath = System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%TEMP%"), "Logs", $"AcceptanceTests-{DateTime.Now.ToString("yyyyMMddTHHmmssfff")}.log");

            foreach (var key in testSettings.EnvironmentVariables.Keys)
            {
                Environment.SetEnvironmentVariable(key, testSettings.EnvironmentVariables[key], EnvironmentVariableTarget.Process);
            }

            Environment.SetEnvironmentVariable("ASPNETCORE_URLS", testSettings.BaseUrl);
            Environment.SetEnvironmentVariable("TEST_LOG_PATH", _logPath);

            if (testSettings.InProcess)
            {
                _host = Program
                    .CreateHostBuilder()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {

                    })
                    .Build();

                await _host.StartAsync();
            }
        }

        [TestCleanup]
        public async Task CleanupAcceptanceTestBase()
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
                _host = null;
            }

            TestContext.AddResultFile(_logPath);
        }

        protected T Resolve<T>()
        {
            return (T)ServiceProvider.GetRequiredService(typeof(T));
        }
    }
}
