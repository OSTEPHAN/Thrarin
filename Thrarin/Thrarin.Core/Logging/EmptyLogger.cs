
namespace Thrarin.Logging
{
    using System;

    public sealed class EmptyLogger : ILogger
    {
        public EmptyLogger()
        {
        }

        void ILogger.LogError(string message, string location)
        {
            return;
        }

        void ILogger.LogException(Exception exception, string location)
        {
            return;
        }

        void ILogger.LogInformation(string message, string location)
        {
            return;
        }

        void ILogger.LogTrace(string message, string location)
        {
            return;
        }

        void ILogger.LogWarning(string message, string location)
        {
            return;
        }
    }
}
