using FluentAssertions;
using GreyhamWooHoo.Interceptor.Core.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreyhamWooHoo.Interceptor.Core.UnitTests.BeforeExecution
{
    [TestClass]
    public class BeforeExecutionTests
    {
        private BeforeExecutionTestImplementation _originalImplementation = new BeforeExecutionTestImplementation();

        private InterceptorProxyBuilder<IBeforeExecutionTestInterface> _builder;

        [TestInitialize]
        public void SetupReturnValueTests()
        {
            _builder = new InterceptorProxyBuilder<IBeforeExecutionTestInterface>()
                .For(_originalImplementation);
        }

        [TestMethod]
        public void MethodHasNoParameters()
        {
            // Arrange
            var args = default(object[]);
            var calledBack = false;

            var proxy = _builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionTestInterface.TheMethodWithNoParameters), andCalledbackWith: result =>
            {
                calledBack = true;
                args = result.Args;
            })
            .Build();

            // Act
            proxy.TheMethodWithNoParameters();

            // Assert
            calledBack.Should().BeTrue(because: "the callback should have been invoked. ");
            
            args.Should().NotBeNull(because: "the callback will have been invoked with no arguments. ");
            
            _originalImplementation.Message.Should().Be("Invoked", because: "it is set by the method after the calledback. ");
        }

        [TestMethod]
        public void MethodHasOneParameter()
        {
            // Arrange
            var args = default(object[]);
            var calledBack = false;

            var proxy = _builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionTestInterface.TheMethodWithOneParameter), andCalledbackWith: result =>
            {
                calledBack = true;
                args = result.Args;
            })
            .Build();

            // Act
            proxy.TheMethodWithOneParameter(10);

            // Assert
            calledBack.Should().BeTrue(because: "the callback should have been invoked. ");

            args.Should().NotBeNull(because: "the callback will have been invoked with one arguments. ");
            args.Length.Should().Be(1, because: "there is exactly one parameter in the method that was invoked. ");
            ((int)args[0]).Should().Be(10, because: "that is the value of the parameter passed in. ");
            
            _originalImplementation.Message.Should().Be("Invoked: 10", because: "it is set by the method after the calledback. ");
        }

        [TestMethod]
        public void MethodHasManyParameters()
        {
            // Arrange
            var args = default(object[]);
            var calledBack = false;

            var proxy = _builder.InterceptBeforeExecutionOf(theMethodNamed: nameof(IBeforeExecutionTestInterface.TheMethodWithManyParameters), andCalledbackWith: result =>
            {
                calledBack = true;
                args = result.Args;
            })
            .Build();

            // Act
            proxy.TheMethodWithManyParameters(20, 30);

            // Assert
            calledBack.Should().BeTrue(because: "the callback should have been invoked. ");

            args.Should().NotBeNull(because: "the callback will have been invoked with one arguments. ");
            args.Length.Should().Be(2, because: "there is exactly one parameter in the method that was invoked. ");
            ((int)args[0]).Should().Be(20, because: "that is the value of the parameter passed in. ");
            ((int)args[1]).Should().Be(30, because: "that is the value of the parameter passed in. ");

            _originalImplementation.Message.Should().Be("Invoked: 20 30", because: "it is set by the method after the calledback. ");
        }
    }
}
