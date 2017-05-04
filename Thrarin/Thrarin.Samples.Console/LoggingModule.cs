

namespace Thrarin.Console
{
    using Autofac;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Reflection;

    internal sealed class FactoryLogger : Logging.ILogger
    {
        private readonly ILoggerFactory coreFactory;
        public FactoryLogger(ILoggerFactory coreFactory)
        {
            this.coreFactory = coreFactory;
        }

        private ILogger GetLogger(string location)
        {
            var className = location.Split('.').FirstOrDefault();
            var fullyClassName = string.Format("{0}.{1}", this.GetType().Namespace, className);
            var classType = Type.GetType(fullyClassName);
            var logger = this.coreFactory.CreateLogger(classType);
            return logger as ILogger;
        }

        void Logging.ILogger.LogError(string message, string location)
        {
            this.GetLogger(location).LogError(message);
        }

        void Logging.ILogger.LogException(System.Exception exception, string location)
        {
            this.GetLogger(location).LogCritical(exception.Message);
        }

        void Logging.ILogger.LogInformation(string message, string location)
        {
            this.GetLogger(location).LogInformation(message);
        }

        void Logging.ILogger.LogTrace(string message, string location)
        {
            this.GetLogger(location).LogTrace(message);
        }

        void Logging.ILogger.LogWarning(string message, string location)
        {
            this.GetLogger(location).LogWarning(message);
        }
    }

    internal sealed class LoggingModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FactoryLogger>().As<Logging.ILogger>();
        }
    }
}
