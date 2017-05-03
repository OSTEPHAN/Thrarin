namespace Thrarin.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IMemoryCacheProvider : ICacheProvider
    {
    }

    public abstract class MemoryCacheProvider : IMemoryCacheProvider
    {
        private readonly List<Tuple<string, object, DateTimeOffset>> memoryContext;

        IQueryable<CacheItem> ICacheProvider.Query =>
            this.memoryContext
                .Where(t => t.Item3 > DateTimeOffset.Now)
                .Select(t => new CacheItem() { Key = t.Item1, Value = t.Item2 })
                .AsQueryable();

        protected MemoryCacheProvider(List<Tuple<string, object, DateTimeOffset>> memoryContext)
        {
            this.memoryContext = memoryContext;
        }

        void ICacheProvider.Set(CacheItem cacheItem, DateTimeOffset absoluteExpiration)
        {
            if (DateTimeOffset.Now >= absoluteExpiration)
            {
                return;
            }

            var found = this.memoryContext.FirstOrDefault(t => t.Item1 == cacheItem.Key);
            if (null != found)
            {
                this.memoryContext.Remove(found);
            }

            this.memoryContext.Add(Tuple.Create(cacheItem.Key, cacheItem.Value, absoluteExpiration));
        }
    }
}
