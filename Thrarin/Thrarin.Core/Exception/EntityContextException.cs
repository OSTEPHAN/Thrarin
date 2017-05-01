
namespace Thrarin.Storage
{
    using System;

    public abstract class EntityContextException : Thrarin.Exception
    {
        protected EntityContextException(string message) : base(message)
        {
        }
        protected EntityContextException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public sealed class InstallEntityContextException : EntityContextException
    {
        public InstallEntityContextException(string message) : base(message)
        {
        }
        public InstallEntityContextException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public InstallEntityContextException() : this("Failure while installing data context", new InvalidOperationException())
        {
        }
    }
}
