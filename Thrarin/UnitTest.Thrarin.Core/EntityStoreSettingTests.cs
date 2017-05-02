
namespace Thrarin.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    using Configuration;
    using Storage;

    [TestClass]
    public partial class EntityStoreSettingTests : AbstractTests
    {
        [TestMethod]
        private void UnitTestQueryMethod()
        {
            var settingsProvider = this.serviceCollectionDependencyResolver.Resolve<ISettingsProvider>();
            Assert.AreEqual(0, settingsProvider.Query.Count());

            var position = 0;
            var entityStore = this.serviceCollectionDependencyResolver.Resolve<IEntityStore>();
            var settings = Enumerable
                .Range(1, 100)
                .Select(i => entityStore.Create<Setting>())
                .Select(s => { ++position; s.Key = position.ToString(); s.Value = position.ToString(); return s; })
                .ToList();
            entityStore.SaveChanges();
            Assert.AreEqual(100, settingsProvider.Query.Count());

            var settingAsInt = settingsProvider.Get<int>("50");
            Assert.AreEqual(50, settingAsInt);

            var fromRepository = entityStore.Query<Setting>().Single(s => s.Key == "50");
            fromRepository.Value = "true";
            entityStore.SaveChanges();

            var settingAsBoolean = settingsProvider.Get<bool>("50");
            Assert.IsTrue(settingAsBoolean);

            entityStore.Query<Setting>().ToList().ForEach(entityStore.Delete);
        }
    }
}
