
namespace Thrarin.Configuration
{
    using Storage;

    public class Setting : IEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
