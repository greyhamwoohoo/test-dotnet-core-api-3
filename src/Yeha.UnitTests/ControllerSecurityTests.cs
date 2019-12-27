using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Yeha.Api.Controllers;

namespace Yeha.UnitTests
{
    /// <summary>
    /// This tests ensures that every Controller has an [Authorize] attribute
    /// All controllers that do NOT have a [Authorize] attribute must be explicitly declared in this test. This prevents accidents getting through to Production - such as [Authorize] being removed for local development but not being added back in. 
    /// All methods with an [AllowAnonymous] attribute must be explicitly declared in this test. Once again: this prevents accidents. 
    /// </summary>
    [TestClass]
    public class ControllerSecurityTests
    {
        // We need to take a strong reference on something in the API so we can find it in the AppDomain
        public Type ProductControllerType = typeof(ProductsController);

        public const string FILENAME_PREFIX = "Yeha";

        public readonly string[] MethodsThatAreAllowedToBeAllowAnonymous = new string[] 
        {
            "Yeha.Api.Controllers.PingAllowAnonymousController.Get",
            "Yeha.Api.Controllers.PingAllowAnonymousController.Delete"
        };

        public readonly Type[] ControllersThatAreAllowedToBeNonAuthorized = new Type[]
        {
            typeof(ProductsController),
            typeof(PrimitivesController)
        };

        private IEnumerable<Type> _controllers;
        private IEnumerable<Type> _authorizedControllers;

        [TestInitialize]
        public void SetupControllerSecurityTests()
        {
            var candidateAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.IsDynamic)
                .Where(assembly => System.IO.Path.GetFileName(assembly.Location).StartsWith(FILENAME_PREFIX));

            _controllers = GetAllControllers(fromAssemblies: candidateAssemblies);
            _authorizedControllers = GetAuthorizedControllers(fromControllers: _controllers);
        }

        [TestMethod]
        public void Sanity_EnsureThereIsAtLeastOneController()
        {
            _controllers.Count().Should().BeGreaterThan(0, because: "there should be at least one controller defined somewhere. If not, this algorithm might not be finding the assembly. ");
        }

        [TestMethod]
        public void Sanity_EnsureThereIsAtLeastOneAuthorizedController()
        {
            _authorizedControllers.Count().Should().BeGreaterThan(0, because: "there should be at least one controller with the [Authorize] attribute defined somewhere. If not, this algorithm might not be finding the assembly. ");
        }

        [TestMethod]
        public void ByConvention_AllControllersMustBeAuthorizedUnlessExplicitlyExcluded()
        {
            // Arrange
            var controllersThatAreNotAuthorizedAndNotExplicitlyExcludedFromAuthorization = _controllers
                .Except(_authorizedControllers)
                .Except(ControllersThatAreAllowedToBeNonAuthorized);

            // Assert
            controllersThatAreNotAuthorizedAndNotExplicitlyExcludedFromAuthorization.Count().Should().Be(0, because: $"every controller should be explicitly Authorized or explicitly excluded via the 'ControllersThatAreAllowedToBeNonAuthorized' array. The following controllers do not currently obey those rules - either add the Authorize attribute or allow them to be exposed to the work by adding it to the 'ControllersThatAreAllowedToBeNonAuthorized' array: {string.Join(',', controllersThatAreNotAuthorizedAndNotExplicitlyExcludedFromAuthorization)}");
        }

        [TestMethod]
        public void ByConvention_NoMethodsCanBeAllowAnonymous_UnlessExplicitlyDeclaredInThisTest()
        {
            // Arrange
            var allMethods = GetAllMethods(_controllers).Where(m => IsAllowAnonymousMethod(m));

            var methodsNotExcluded = allMethods.Select(m => ToFullyQualifiedMethodName(m)).Except(MethodsThatAreAllowedToBeAllowAnonymous);

            // Assert
            methodsNotExcluded.Count().Should().Be(0, because: $"no methods should be marked as AllowAnonymous unless they are explicitly declared in the 'MethodsThatAreAllowedToBeAllowAnonymous' array. The following methods either need to have their AllowAnonymous attribute removed to need declared in the 'MethodsThatAreAllowedToBeAllowAnonymous' array: {string.Join(',', methodsNotExcluded)}");
        }

        private IEnumerable<Type> GetAllControllers(IEnumerable<Assembly> fromAssemblies)
        {
            var result = new List<Type>();

            foreach(var assembly in fromAssemblies)
            {
                result.AddRange(GetAllControllers(fromAssembly: assembly));
            };

            return result;
        }

        private IEnumerable<Type> GetAllControllers(Assembly fromAssembly)
        {
            var controllers = fromAssembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ControllerBase)));
            return controllers;
        }

        private IEnumerable<Type> GetAuthorizedControllers(IEnumerable<Type> fromControllers)
        {
            var authorizedControllers = fromControllers.Where(c => c.GetCustomAttributes<AuthorizeAttribute>().Count() > 0);
            return authorizedControllers;
        }


        private IEnumerable<MethodInfo> GetAllMethods(IEnumerable<Type> controllers)
        {
            var result = new List<MethodInfo>();
            foreach(var controller in controllers)
            {
                result.AddRange(controller.GetMethods());
            }
            return result;
        }

        private bool IsAllowAnonymousMethod(MethodInfo methodInfo)
        {
            if (null == methodInfo) return false;

            return methodInfo.GetCustomAttributes<AllowAnonymousAttribute>().Count() > 0;
        }

        private string ToFullyQualifiedMethodName(MethodInfo methodInfo)
        {
            return $"{methodInfo.DeclaringType.FullName}.{methodInfo.Name}";
        }
    }
}
