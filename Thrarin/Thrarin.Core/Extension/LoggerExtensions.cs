
namespace Thrarin.Logging
{
    using System.IO;
    using System.Runtime.CompilerServices;

    public static class LoggerExtension
    {
        public static void LogException
            (this ILogger logger
            , System.Exception exception
            , [CallerFilePath] string callerFilePath = ""
            , [CallerMemberName]string callerMemberName = ""
            , [CallerLineNumber] int callerLineNumber = 0)
        {
            var callerClassName = Path.GetFileNameWithoutExtension(callerFilePath);
            var stackTrace = string.Format("{0}.{1} [{2}]", callerClassName, callerMemberName, callerLineNumber);
            logger.LogException(exception, stackTrace);
        }

        public static void LogError
            (this ILogger logger
            , string message
            , [CallerFilePath] string callerFilePath = ""
            , [CallerMemberName]string callerMemberName = ""
            , [CallerLineNumber] int callerLineNumber = 0)
        {
            var callerClassName = Path.GetFileNameWithoutExtension(callerFilePath);
            var stackTrace = string.Format("{0}.{1} [{2}]", callerClassName, callerMemberName, callerLineNumber);
            logger.LogError(message, stackTrace);
        }

        public static void LogWarning
            (this ILogger logger
            , string message
            , [CallerFilePath] string callerFilePath = ""
            , [CallerMemberName]string callerMemberName = ""
            , [CallerLineNumber] int callerLineNumber = 0)
        {
            var callerClassName = Path.GetFileNameWithoutExtension(callerFilePath);
            var stackTrace = string.Format("{0}.{1} [{2}]", callerClassName, callerMemberName, callerLineNumber);
            logger.LogWarning(message, stackTrace);
        }

        public static void LogInformation
            (this ILogger logger
            , string message
            , [CallerFilePath] string callerFilePath = ""
            , [CallerMemberName]string callerMemberName = ""
            , [CallerLineNumber] int callerLineNumber = 0)
        {
            var callerClassName = Path.GetFileNameWithoutExtension(callerFilePath);
            var stackTrace = string.Format("{0}.{1} [{2}]", callerClassName, callerMemberName, callerLineNumber);
            logger.LogInformation(message, stackTrace);
        }

        public static void LogTrace
            (this ILogger logger
            , string message
            , [CallerFilePath] string callerFilePath = ""
            , [CallerMemberName]string callerMemberName = ""
            , [CallerLineNumber] int callerLineNumber = 0)
        {
            var callerClassName = Path.GetFileNameWithoutExtension(callerFilePath);
            var stackTrace = string.Format("{0}.{1} [{2}]", callerClassName, callerMemberName, callerLineNumber);
            logger.LogTrace(message, stackTrace);
        }

    }
}
