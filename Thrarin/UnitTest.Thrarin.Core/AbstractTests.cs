
namespace Thrarin.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    using Caching;
    using Configuration;
    using Logging;
    using Storage;

    public abstract class AbstractTests : IDisposable
    {
        private bool disposedValue = false;
        protected readonly IDependencyResolver serviceCollectionDependencyResolver;
        public AbstractTests()
        {
            this.serviceCollectionDependencyResolver = new ServiceCollectionDependencyResolver() as IDependencyResolver;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.TestInitialize(this.serviceCollectionDependencyResolver);
        }

        private void TestInitialize(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.Register<DummyData>();
            dependencyResolver.Register<ILogger>(new EmptyLogger());
            dependencyResolver.Register<IEntityStore, DummyContext>();
            dependencyResolver.Register<IEntityQuery>(() => dependencyResolver.Resolve<IEntityStore>());
            dependencyResolver.Register<ISettingsProvider>(() => new EntityStoreSettingsProvider(dependencyResolver.Resolve<IEntityStore>()));
            dependencyResolver.Register<DummyCacheProvider>(new DummyCacheProvider());
            dependencyResolver.Register<ICacheProvider>(() => dependencyResolver.Resolve<DummyCacheProvider>());
            dependencyResolver.Configure();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposedValue)
            {
                return;
            }
            if (disposing)
            {
            }
            this.disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
