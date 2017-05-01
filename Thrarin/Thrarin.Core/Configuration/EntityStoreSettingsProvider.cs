
namespace Thrarin.Configuration
{
    using Storage;
    using System.Linq;

    public sealed class EntityStoreSettingsProvider : ISettingsProvider
    {

        private readonly IEntityStore entityStore;
        public EntityStoreSettingsProvider(IEntityStore entityStore)
        {
            this.entityStore = entityStore;
        }

        IQueryable<Setting> ISettingsProvider.Query
        {
            get { return this.entityStore.Query<Setting>(); }
        }
    }
}
