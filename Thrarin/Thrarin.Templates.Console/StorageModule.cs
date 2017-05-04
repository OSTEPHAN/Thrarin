
namespace Thrarin.Console
{
    using Autofac;
    using Configuration;
    using Microsoft.EntityFrameworkCore;
    using Storage;

    internal class DataContext : EntityFrameworkContext
    {
        public DbSet<Setting> Settings { get; set; }
        private static readonly DbContextOptions<DataContext> Options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite("Data Source=ConsoleSample.db")
            .Options;
        public DataContext() : base()
        {
        }
        public DataContext(DbContextOptions<DataContext> dbConnection) : base(dbConnection)
        {
        }
        protected override string SchemaName => "CONSOLE";
        protected override string ConnectionString => @"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;";
    }
    internal sealed class SettingSqlMapping : Storage.EntityFrameworkConfiguration<Configuration.Setting>
    {
        public SettingSqlMapping() : base()
        {
        }
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Configuration.Setting>()
                .HasKey(s => s.Key);
        }
    }

    internal sealed class StorageModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataContext>().As<EntityFrameworkContext>();
            builder.RegisterType<EntityFrameworkStorage>().As<IEntityStore>();
            builder.Register<IEntityQuery>(ctx => ctx.Resolve<IEntityStore>())
;        }
    }
}
