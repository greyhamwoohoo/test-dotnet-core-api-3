using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Yeha.Api.AcceptanceTests.Infrastructure
{
    /// <summary>
    /// </summary>
    public static class ContainerSingleton
    {
        public static IServiceProvider BuildContainer(string testExecutionContextFilename)
        {
            var services = new ServiceCollection();

            var configuration = BuildTestExecutionContext(testExecutionContextFilename);

            var testSettings = configuration.GetSection("TestSettings").Get<TestSettings>();
            services.AddSingleton(testSettings);

            // TODO: Configure Container

            var result = services.BuildServiceProvider();
            return result;
        }

        private static IConfigurationRoot BuildTestExecutionContext(string testExecutionContextFilename)
        {
            var testExecutionContextRelativePath = Path.Combine("TestExecutionContexts", $"{testExecutionContextFilename}");
            var testExecutionContextFullPath = Path.Combine(Directory.GetCurrentDirectory(), testExecutionContextRelativePath);
            if (!File.Exists(testExecutionContextFullPath))
            {
                Assert.Fail($"The TestExecutionContext file does not exist at '{testExecutionContextFullPath}");
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine("TestExecutionContexts", "testsettings.json"), optional: false)
                .AddJsonFile(testExecutionContextRelativePath, optional: true)
                .Build();

            return configuration;
        }
    }
}
