
namespace Thrarin.Tests
{
    using Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Storage;
    using System;
    using System.Linq;

    public class DataContext : EntityFrameworkContext
    {
        public DbSet<Setting> Settings { get; set; }
        private static readonly DbContextOptions<DataContext> Options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
            .Options;
        public DataContext() : base()
        {
        }
        public DataContext(DbContextOptions<DataContext> dbConnection) : base(dbConnection)
        {
        }
        protected override string SchemaName => "TEST";
        protected override string ConnectionString => @"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;";

        internal class DataContextSeed : EntityFrameworkSeed<DataContext>
        {
            public DataContextSeed(DataContext dataContext) : base(dataContext)
            {
            }
            protected override void SeedForDevelopment()
            {
                this.EntityFrameworkContext.Settings.ToList().ForEach(s => this.EntityFrameworkContext.Remove(s));
                this.EntityFrameworkContext.SaveChanges();
            }
            protected override void SeedForStaging()
            {
                throw new NotImplementedException();
            }
            protected override void SeedForProduction()
            {
                throw new NotImplementedException();
            }
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
            dependencyResolver.Register<IEntityStore>(new EntityFrameworkStorage(new DataContext()));
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
