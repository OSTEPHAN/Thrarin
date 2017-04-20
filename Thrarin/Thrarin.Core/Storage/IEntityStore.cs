
namespace Thrarin.Storage
{
    public interface IEntityStore : IEntityQuery, IEntityContext
    {
        T Create<T>() where T : class, IEntity;
        void Update<T>(T entity) where T : class, IEntity;
        void Delete<T>(T entity) where T : class, IEntity;
        void SaveChanges();
    }
}
