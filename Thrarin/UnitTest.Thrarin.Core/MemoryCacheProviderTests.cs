
namespace Thrarin.Tests
{
    using Caching;
    using Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public class MemoryCacheProviderTests : AbstractTests
    {
        [TestMethod]
        public void TestQueryMethod()
        {
            var dummyCacheProvider = this.serviceCollectionDependencyResolver.Resolve<DummyCacheProvider>();
            var cacheProvider = this.serviceCollectionDependencyResolver.Resolve<ICacheProvider>();
            var cacheItems = cacheProvider.Query;

            Assert.IsTrue(false == cacheItems.Any());
            dummyCacheProvider.Cache.Add(
                System.Tuple.Create<string, object, System.DateTimeOffset>("one", 1, System.DateTimeOffset.Now.AddMinutes(1)));

            Assert.IsTrue(1 == cacheItems.Count());
            Assert.AreEqual("one", cacheItems.First().Key);
            Assert.AreEqual(1, cacheItems.First().Value);

            dummyCacheProvider.Cache.Add(
                System.Tuple.Create<string, object, System.DateTimeOffset>("two", 2, System.DateTimeOffset.Now.AddMinutes(1)));
            dummyCacheProvider.Cache.Add(
                System.Tuple.Create<string, object, System.DateTimeOffset>("three", 3, System.DateTimeOffset.Now.AddMinutes(1)));

            Assert.IsTrue(3 == cacheItems.Count());
            Assert.AreEqual(2, cacheItems.First(c => c.Key == "two").Value);

            dummyCacheProvider.Cache.Clear();
        }

        [TestMethod]
        public void TestGetSetMethod()
        {
            var cacheProvider = this.serviceCollectionDependencyResolver.Resolve<ICacheProvider>();
            var cacheItems = cacheProvider.Query;

            cacheProvider.Set("valueToSet", "testValue");
            Assert.AreEqual("testValue", cacheProvider.Get("valueToSet"));

            cacheProvider.Set("valueToSet", "eulaVtset");
            Assert.AreNotEqual("testValue", cacheProvider.Get("valueToSet"));
            Assert.AreEqual("eulaVtset", cacheProvider.Get("valueToSet"));

            (cacheProvider as DummyCacheProvider).Cache.Clear();
        }
    }
}
