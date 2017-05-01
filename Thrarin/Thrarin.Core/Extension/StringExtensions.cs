
namespace Thrarin
{
    public static class StringExtensions
    {
        public static T Parse<T>(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            try
            {
                return (T)System.Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
    }
}
