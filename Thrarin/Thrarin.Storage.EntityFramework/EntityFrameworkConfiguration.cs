
namespace Thrarin.Storage
{
    public abstract class EntityFrameworkConfiguration<T> where T : class, IEntity
    {
        protected EntityFrameworkConfiguration()
        {
        }

        public abstract void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder);
    }
}
