
namespace Thrarin.Caching
{
    using System;
    using System.Linq;

    public interface ICacheProvider
    {
        IQueryable<CacheItem> Query { get; }
        void Set(CacheItem cacheItem, DateTimeOffset absoluteExpiration);
    }
}
