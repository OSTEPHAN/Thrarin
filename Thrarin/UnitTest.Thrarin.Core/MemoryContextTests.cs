
namespace Thrarin.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    using Configuration;
    using Storage;

    [TestClass]
    public partial class MemoryContextTests : AbstractTests
    {
        [TestMethod]
        public void UnitTestInstallMethod()
        {
            var entityStore = this.serviceCollectionDependencyResolver.Resolve<IEntityStore>();
            var exceptionRaised = false;
            try
            {
                entityStore.Install(string.Empty, string.Empty);
            }
            catch (EntityContextException)
            {
                exceptionRaised = true;
            }
            Assert.IsTrue(exceptionRaised);
        }

        [TestMethod]
        public void UnitTestCreateMethod()
        {
            var entityStore = this.serviceCollectionDependencyResolver.Resolve<IEntityStore>();
            
            var dummyDataFromResolver = this.serviceCollectionDependencyResolver.Resolve<DummyData>();
            dummyDataFromResolver.Id = System.Guid.NewGuid();

            var dummyDataFromRepository = entityStore.Create<DummyData>();
            entityStore.SaveChanges();

            dummyDataFromRepository = entityStore.Query<DummyData>().LastOrDefault(d => d.Value == 0);
            Assert.IsNotNull(dummyDataFromRepository);
            Assert.AreNotEqual(dummyDataFromRepository.Id, dummyDataFromResolver.Id);

            dummyDataFromRepository = entityStore.Create<DummyData>();
            dummyDataFromRepository.Id = dummyDataFromResolver.Id;
            entityStore.SaveChanges();

            dummyDataFromRepository = entityStore.Query<DummyData>().LastOrDefault(d => d.Value == 0);
            Assert.AreEqual(dummyDataFromRepository.Id, dummyDataFromResolver.Id);

            entityStore.Query<DummyData>().ToList().ForEach(entityStore.Delete);
        }

        [TestMethod]
        public void UnitTestReadMethod()
        {
            var position = 0;
            var entityStore = this.serviceCollectionDependencyResolver.Resolve<IEntityStore>();

            Enumerable
                .Range(1, 100)
                .Select(i => entityStore.Create<DummyData>())
                .Select(d => { d.Id = System.Guid.NewGuid(); d.Value = ++position; return d; })
                .ToList();
            entityStore.SaveChanges();

            var dummy = entityStore.Query<DummyData>().FirstOrDefault(d => d.Value > 0);
            Assert.IsNotNull(dummy);
            Assert.AreEqual(1, dummy.Value);

            dummy = entityStore.Query<DummyData>().LastOrDefault(d => d.Value > 0);
            Assert.IsNotNull(dummy);
            Assert.AreEqual(100, dummy.Value);

            var entities = entityStore.Query<DummyData>(d => d.Value).Where(d => d.Value > 0);
            Assert.AreEqual(100, entities.Count());

            entityStore.Query<DummyData>().ToList().ForEach(entityStore.Delete);
        }

        [TestMethod]
        public void UnitTestUpdateMethod()
        {
            var entityStore = this.serviceCollectionDependencyResolver.Resolve<IEntityStore>();
            var guid = System.Guid.NewGuid();
            var value = 0;

            var dummyDataFromRepository = entityStore.Create<DummyData>();
            dummyDataFromRepository.Id = guid;
            dummyDataFromRepository.Value = value;
            entityStore.SaveChanges();

            dummyDataFromRepository = entityStore.Query<DummyData>().First(d => d.Id == guid);
            Assert.AreEqual(value++, dummyDataFromRepository.Value);
            
            dummyDataFromRepository = entityStore.Query<DummyData>().First(d => d.Id == guid);
            Assert.AreNotEqual(value, dummyDataFromRepository.Value);

            dummyDataFromRepository.Value = value;
            entityStore.SaveChanges();

            dummyDataFromRepository = entityStore.Query<DummyData>().First(d => d.Id == guid);
            Assert.AreEqual(value, dummyDataFromRepository.Value);

            entityStore.Query<DummyData>().ToList().ForEach(entityStore.Delete);
        }

        [TestMethod]
        public void UnitTestDeleteMethod()
        {
            var entityStore = this.serviceCollectionDependencyResolver.Resolve<IEntityStore>();
            var guid = System.Guid.NewGuid();

            var dummyDataFromRepository = entityStore.Query<DummyData>().FirstOrDefault(d => d.Id == guid);
            Assert.IsNull(dummyDataFromRepository);

            dummyDataFromRepository = entityStore.Create<DummyData>();
            dummyDataFromRepository.Id = guid;
            entityStore.SaveChanges();

            dummyDataFromRepository = entityStore.Query<DummyData>().FirstOrDefault(d => d.Id == guid);
            Assert.IsNotNull(dummyDataFromRepository);

            entityStore.Delete<DummyData>(dummyDataFromRepository);
            entityStore.SaveChanges();

            dummyDataFromRepository = entityStore.Query<DummyData>().FirstOrDefault(d => d.Id == guid);
            Assert.IsNull(dummyDataFromRepository);

            entityStore.Query<DummyData>().ToList().ForEach(entityStore.Delete);
        }
    }
}
