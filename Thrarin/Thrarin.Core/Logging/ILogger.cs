
namespace Thrarin.Logging
{
    using System;

    public interface ILogger
    {
        void LogException(Exception exception, string location);
        void LogError(string message, string location);
        void LogWarning(string message, string location);
        void LogInformation(string message, string location);
        void LogTrace(string message, string location);
    }
}
