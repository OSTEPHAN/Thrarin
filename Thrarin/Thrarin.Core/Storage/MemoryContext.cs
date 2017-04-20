
namespace Thrarin.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections;

    public interface IMemoryQuery : IEntityQuery
    {
    }

    public abstract class MemoryContext : IEntityStore, IMemoryQuery
    {
        private readonly Dictionary<Type, IEnumerable> memoryContext;

        protected MemoryContext(Dictionary<Type, IEnumerable> memoryContext)
        {
            this.memoryContext = memoryContext;
        }

        public virtual T Create<T>() where T : class, IEntity
        {
            var entity = Activator.CreateInstance<T>();
            if (false == this.memoryContext.Keys.Any(k => k == typeof(T)))
            {
                this.memoryContext[typeof(T)] = new List<T>() { }.AsEnumerable();
            }

            var enumerable = this.memoryContext[typeof(T)];
            enumerable = enumerable.Cast<T>().Concat(new List<T>() { entity }).AsEnumerable();
            this.memoryContext[typeof(T)] = enumerable;
            return entity;
        }

        public virtual void Delete<T>(T entity) where T : class, IEntity
        {
            if (false == this.memoryContext.Keys.Any(k => k == typeof(T)))
            {
                this.memoryContext[typeof(T)] = new List<T>() { }.AsEnumerable();
            }

            var enumerable = this.memoryContext[typeof(T)];
            var list = enumerable.Cast<T>().ToList();
            if (0 <= list.IndexOf(entity))
            {
                list.Remove(entity);
            }
            this.memoryContext[typeof(T)] = list.AsEnumerable();
        }

        public virtual void Install(string plateform, string environment)
        {
//            throw new InstallEntityContextException();
        }

        public virtual IQueryable<T> Query<T>(string[] includes) where T : class, IEntity
        {
            var memoryContextKeys = this.memoryContext.Keys.Where(k => k == typeof(T));

            if (false == memoryContextKeys.Any())
            {
                return new List<T>() { }.AsQueryable();
            }
            
            return memoryContextKeys.SelectMany(k => this.memoryContext[k].Cast<T>()).AsQueryable();
        }

        public virtual void SaveChanges()
        {
        }

        public virtual void Update<T>(T entity) where T : class, IEntity
        {
        }
    }
}
