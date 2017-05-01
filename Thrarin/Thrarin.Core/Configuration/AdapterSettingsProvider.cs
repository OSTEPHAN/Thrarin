
namespace Thrarin.Configuration
{
    using System.Linq;

    public sealed class AdapterSettingsProvider : ISettingsProvider
    {
        private readonly ISettingsProvider adapter;
        private readonly ISettingsProvider adaptee;
        public AdapterSettingsProvider(ISettingsProvider adapter, ISettingsProvider adaptee)
        {
            this.adapter = adapter;
            this.adaptee = adaptee;
        }

        IQueryable<Setting> ISettingsProvider.Query
        {
            get
            {
                var adapterSettingKeys = adapter.Query.Select(setting => setting.Key).ToList();
                var adapteeSettingKeys = adaptee.Query.Select(setting => setting.Key).Except(adapterSettingKeys).ToList();

                var adapterSettings = this.adapter.Query.ToList();
                var adapteeSettings = this.adaptee.Query.Where(s => adapteeSettingKeys.Contains(s.Key)).ToList();

                return adapterSettings.Concat(adapteeSettings).AsQueryable();
            }
        }
    }
}
