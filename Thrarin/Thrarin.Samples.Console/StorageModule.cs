
namespace Thrarin.Console
{
    using Autofac;
    using Configuration;
    using Microsoft.EntityFrameworkCore;
    using Storage;

    internal class DataContext : EntityFrameworkContext
    {
        public DbSet<Setting> Settings { get; set; }
        public DataContext() : base()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Console.db");
        }
        protected override string SchemaName => string.Empty;
        protected override string ConnectionString => string.Empty;
    }

    internal sealed class SettingSqlMapping : EntityFrameworkConfiguration<Setting>
    {
        public SettingSqlMapping() : base()
        {
        }
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>()
                .HasKey(s => s.Key);
        }
    }

    internal sealed class StorageModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataContext>().As<EntityFrameworkContext>().SingleInstance();
            builder.RegisterType<EntityFrameworkStorage>().As<IEntityStore>();
            builder.Register<IEntityQuery>(ctx => ctx.Resolve<IEntityStore>())
;        }
    }
}
