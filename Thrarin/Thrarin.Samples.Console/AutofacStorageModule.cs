
namespace Thrarin.Console
{
    using Autofac;
    using Configuration;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Storage;

    public class Blog : IEntity
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class Post : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }

    internal class DataContext : EntityFrameworkContext
    {
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        private readonly ISettingsProvider settingsProvider;
        public DataContext(ISettingsProvider settingsProvider) : base()
        {
            this.settingsProvider = settingsProvider;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = this.settingsProvider.Get<string>("ConnectionStrings:Sample");
            optionsBuilder.UseSqlite(connectionString);
        }
        protected override string SchemaName => string.Empty;
        protected override string ConnectionString => string.Empty;
    }

    internal class DataContextSeed : EntityFrameworkSeed<DataContext>
    {
        public DataContextSeed(DataContext dataContext) : base(dataContext)
        {
        }
        protected override void SeedForDevelopment()
        {
            this.EntityFrameworkContext.Blogs.ToList().ForEach(s => this.EntityFrameworkContext.Remove(s));
            this.EntityFrameworkContext.Blogs.Add(new Blog()
            {
                Url = "www.nuget.org",
                Posts = new List<Post>()
                {
                    new Post() { Title = "First", Content = "lorem ipsum dolor sit amet"},
                    new Post() { Title = "Second", Content = "lorem ipsum dolor sit amet"}
                }
            });
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

    internal sealed class AutofacStorageModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataContext>().As<EntityFrameworkContext>().SingleInstance();
            builder.RegisterType<EntityFrameworkStorage>().As<IEntityStore>();
            builder.Register<IEntityQuery>(ctx => ctx.Resolve<IEntityStore>())
;        }
    }
}
