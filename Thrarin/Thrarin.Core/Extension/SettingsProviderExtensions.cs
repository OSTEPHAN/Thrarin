
namespace Thrarin.Configuration
{
    using System.Linq;

    public static class SettingsQueryExtensions
    {
        public static Setting Get(this ISettingsProvider settingsQuery, string key)
        {
            return settingsQuery.Query.FirstOrDefault(s => s.Key == key)
                ?? new Setting() { Key = key, Value = string.Empty, };
        }

        public static T Get<T>(this ISettingsProvider settingsQuery, string key)
        {
            return settingsQuery.Get(key).Value.Parse<T>();
        }
    }
}
