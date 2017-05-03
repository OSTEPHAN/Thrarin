
namespace Thrarin.Storage
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;

    public sealed class EntityFrameworkStorage : IEntityStore
    {
        private readonly EntityFrameworkContext entityFrameworkContext;
        public EntityFrameworkStorage(EntityFrameworkContext entityFrameworkContext)
        {
            this.entityFrameworkContext = entityFrameworkContext;
        }

        T IEntityStore.Create<T>()
        {
            var entity = Activator.CreateInstance<T>();
            this.entityFrameworkContext.Set<T>().Add(entity);
            return entity;
        }

        void IEntityStore.Delete<T>(T entity)
        {
            this.entityFrameworkContext.Set<T>().Remove(entity);
        }

        void IEntityContext.Install(string plateform, string environment)
        {
            (this.entityFrameworkContext as IEntityContext).Install(plateform, environment);
        }

        IQueryable<T> IEntityQuery.Query<T>(string[] includes)
        {
            var query = this.entityFrameworkContext.Set<T>().AsQueryable();
            if (includes == null || includes.Length < 1)
            {
                return query;
            }
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }

        void IEntityStore.SaveChanges()
        {
            this.entityFrameworkContext.SaveChanges();
        }

        void IEntityStore.Update<T>(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
