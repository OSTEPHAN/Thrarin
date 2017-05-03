﻿
namespace Thrarin.Storage
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Reflection;

    public abstract class EntityFrameworkContext : DbContext, IEntityContext
    {
        protected abstract string SchemaName { get; }

        private readonly string connectionString;
        protected EntityFrameworkContext(string connectionString) : base()
        {
            this.connectionString = connectionString;
        }
        protected EntityFrameworkContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(this.SchemaName);

            var dbContextAssemblyTypes = this.GetType().GetTypeInfo().Assembly.GetTypes();
            Func<Type, bool> isBaseTypeGenericType = t => t.GetTypeInfo().BaseType?.GetTypeInfo().IsGenericType ?? false;
            Func<Type, Type> baseTypeAsGenericType = t => t.GetTypeInfo().BaseType?.GetGenericTypeDefinition() ?? null;
            var configurationTypes = dbContextAssemblyTypes.Where(t =>
                isBaseTypeGenericType(t) &&
                baseTypeAsGenericType(t) == typeof(EntityFrameworkConfiguration<>));

            var methodInfo = typeof(EntityFrameworkConfiguration<>).GetMethod("OnModelCreating");
            configurationTypes
                .Select(t => Activator.CreateInstance(t))
                .ToList()
                .ForEach(i => methodInfo.Invoke(i, new object[] { modelBuilder }));
        }

        void IEntityContext.Install(string plateform, string environment)
        {
            this.InstallContext(plateform, environment);
        }
    }
}
