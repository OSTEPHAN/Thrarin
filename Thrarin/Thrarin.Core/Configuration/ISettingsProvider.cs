
namespace Thrarin.Configuration
{
    using System.Linq;

    public interface ISettingsProvider
    {
        IQueryable<Setting> Query { get; }
    }
}
