
namespace Thrarin.Caching
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class CacheProviderExtensions
    {
        public static object Get(this ICacheProvider cacheProvider, string key)
        {
            var cacheItem = cacheProvider.Query.FirstOrDefault(c => c.Key == key) ?? new CacheItem() { Value = null, };
            return cacheItem.Value;
        }

        public static object Get(this ICacheProvider cacheProvider, [CallerFilePath]string callerFilePath = "", [CallerMemberName]string callerMemberName = "")
        {
            var callerClassName = Path.GetFileNameWithoutExtension(callerFilePath);
            var key = string.Format("{0}_{1}", callerClassName, callerMemberName);
            return cacheProvider.Get(key);
        }

        public static void Set(this ICacheProvider cacheProvider, string key, object value, double minutes = 1)
        {
            var dateTimeOffset = DateTimeOffset.Now.AddMinutes(minutes);
            cacheProvider.Set(new CacheItem() { Key = key, Value = value }, dateTimeOffset);
        }

        public static void Set(this ICacheProvider cacheProvider, object value, double minutes = 1, [CallerFilePath]string callerFilePath = "", [CallerMemberName]string callerMemberName = "")
        {
            var callerClassName = Path.GetFileNameWithoutExtension(callerFilePath);
            var key = string.Format("{0}_{1}", callerClassName, callerMemberName);
            cacheProvider.Set(key, value, minutes);
        }
    }
}
