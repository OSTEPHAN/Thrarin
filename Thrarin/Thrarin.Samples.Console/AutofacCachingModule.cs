
namespace Thrarin.Console
{
    using Autofac;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Reflection;
    using Thrarin.Caching;

    internal sealed class MemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache memoryCache;
        public MemoryCacheProvider(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        IQueryable<CacheItem> ICacheProvider.Query
        {
            get
            {
                var EntriesCollectionProperty = typeof(MemoryCache)
                    .GetFields(BindingFlags.NonPublic|BindingFlags.Instance)
                    .First(p => p.Name == "_entries");
                var entriesCollection = (System.Collections.IDictionary)EntriesCollectionProperty.GetValue(this.memoryCache);
                return entriesCollection
                    .Values                
                    .Cast<ICacheEntry>()
                    .Where(c => c.AbsoluteExpiration?.CompareTo(DateTimeOffset.Now) > 0)
                    .Select(kvp => new CacheItem() { Key = kvp.Key.ToString(), Value = kvp.Value })
                    .AsQueryable();
            }
        }

        void ICacheProvider.Set(CacheItem cacheItem, DateTimeOffset absoluteExpiration)
        {
            this.memoryCache.Set(cacheItem.Key, cacheItem.Value, absoluteExpiration);
        }
    }

    internal sealed class AutofacCachingModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var cacheProvider = new ServiceCollection().AddMemoryCache().BuildServiceProvider();
            builder.Register(ctx => cacheProvider.GetRequiredService<IMemoryCache>());
            builder.RegisterType<MemoryCacheProvider>().As<ICacheProvider>().SingleInstance();
        }
    }
}
