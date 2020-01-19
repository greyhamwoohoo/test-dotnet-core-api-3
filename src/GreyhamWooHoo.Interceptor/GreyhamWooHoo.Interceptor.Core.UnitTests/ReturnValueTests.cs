using FluentAssertions;
using FluentAssertions.Execution;
using GreyhamWooHoo.Interceptor.Core.Builders;
using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.UnitTests.ReturnValue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests
{
    [TestClass]
    public class ReturnValueTests
    {
        private ReturnValueImplementation _originalImplementation = new ReturnValueImplementation();

        private CaptureReturnValueProxyBuilder<IReturnValueInterface> _builder;

        [TestInitialize]
        public void SetupReturnValueTests()
        {
            _builder = new CaptureReturnValueProxyBuilder<IReturnValueInterface>()
                .For(_originalImplementation);
        }

        [TestMethod]
        public void WhenMethodReturnsVoid_WillReturnEmptyResult()
        {
            // Arrange
            var result = default(ICaptureReturnValueResult);

            var proxy = _builder.InterceptReturnValueOf(nameof(IReturnValueInterface.TheVoidMethod), (callbackResult) =>
            {
                result = callbackResult;
            })
            .Build();

            // Act
            proxy.TheVoidMethod();

            // Assert
            _originalImplementation.Message.Should().Be($"Invoked: {nameof(IReturnValueInterface.TheVoidMethod)}", because: "the method should have been invoked before the callback. ");

            result.Should().NotBeNull(because: "the callback should have been invoked with the return value result. ");
            using (var scope = new AssertionScope())
            {
                result.HasReturnValue.Should().BeFalse(because: "a void method has no return value. ");
                result.ReturnValue.Should().BeNull(because: "by design, we ensure that a void method will return null; use HasReturnValue as a discriminator. ");
            }

            result.Rule.Should().NotBeNull(because: "the intercept rule should always be passed to the callback. ");
            result.Rule.MethodName.Should().Be(nameof(IReturnValueInterface.TheVoidMethod));
        }
    }
}
