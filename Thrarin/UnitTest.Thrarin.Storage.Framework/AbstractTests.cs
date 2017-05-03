
namespace Thrarin.Tests
{
    using Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Storage;
    using System;

    public class DataContext : EntityFrameworkContext
    {
        public DbSet<Setting> Settings { get; set; }
        public DataContext() : base(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;")
        {
        }
        public DataContext(DbContextOptions<DataContext> dbConnection) : base(dbConnection)
        {
        }
        protected override string SchemaName
        {
            get { return "TEST"; }
        }
    }

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
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;
            var context = new DataContext(options);
            var storage = new EntityFrameworkStorage(context);

            dependencyResolver.Register<IEntityStore>(storage);
            dependencyResolver.Register<IEntityQuery>(() => dependencyResolver.Resolve<IEntityStore>());
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
