
namespace Thrarin.Tests
{
    using Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EmptyLoggerTests : AbstractTests
    {
        [TestMethod]
        public void UnitTestLogErrorMethod()
        {
            var logger = this.serviceCollectionDependencyResolver.Resolve<ILogger>();
            logger.LogError(string.Empty);
        }

        [TestMethod]
        public void UnitTestLogException()
        {
            var logger = this.serviceCollectionDependencyResolver.Resolve<ILogger>();
            logger.LogException(new System.Exception());
        }

        [TestMethod]
        public void UnitTestLogInformation()
        {
            var logger = this.serviceCollectionDependencyResolver.Resolve<ILogger>();
            logger.LogInformation(string.Empty);
        }

        [TestMethod]
        public void UnitTestLogTrace()
        {
            var logger = this.serviceCollectionDependencyResolver.Resolve<ILogger>();
            logger.LogTrace(string.Empty);
        }

        [TestMethod]
        public void UnitTestLogWarning()
        {
            var logger = this.serviceCollectionDependencyResolver.Resolve<ILogger>();
            logger.LogWarning(string.Empty);
        }
    }
}
