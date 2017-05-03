
namespace Thrarin.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public partial class EntityFrameworkContextTests : AbstractTests
    {
        [TestMethod]
        public void TestInstallWithNoSeedMethod()
        {
            var entityStore = this.serviceCollectionDependencyResolver.Resolve<Storage.IEntityStore>();
            var exceptionRaised = false;
            try
            {
                entityStore.Install(string.Empty, string.Empty);
            }
            catch (Storage.EntityContextException)
            {
                exceptionRaised = true;
            }
            Assert.IsFalse(exceptionRaised);
        }

        [TestMethod]
        public void TestEntityFrameworkCreateMethod()
        {
            var entityStore = this.serviceCollectionDependencyResolver.Resolve<Storage.IEntityStore>();
            entityStore.Install(string.Empty, string.Empty);

            var query = this.serviceCollectionDependencyResolver.Resolve<Storage.IEntityQuery>().Query<Configuration.Setting>();
            Assert.IsFalse(query.Any());

            entityStore = this.serviceCollectionDependencyResolver.Resolve<Storage.IEntityStore>();
            var setting = entityStore.Create<Configuration.Setting>();
            setting.Key = System.Guid.NewGuid().ToString();
            setting.Value = string.Empty;
            entityStore.SaveChanges();

            query = this.serviceCollectionDependencyResolver.Resolve<Storage.IEntityQuery>().Query<Configuration.Setting>();
            Assert.IsTrue(query.Any());
        }
    }
}
