
namespace Thrarin.Storage
{
    using System.Linq;

    public interface IEntityQuery
    {
        IQueryable<T> Query<T>(string[] includes = null) where T : class, IEntity;
    }
}
