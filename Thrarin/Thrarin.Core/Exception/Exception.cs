
namespace Thrarin
{
    public abstract class Exception : System.Exception
    {
        protected Exception() : base()
        {
        }
        protected Exception(string message) : base(message)
        {
        }
        protected Exception(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
