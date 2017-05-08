

namespace Thrarin.Console
{
    using Autofac;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;

    internal sealed class FactoryLogger : Logging.ILogger
    {
        private readonly ILoggerFactory coreFactory;
        public FactoryLogger(ILoggerFactory coreFactory)
        {
            this.coreFactory = coreFactory;
        }

        private ILogger GetLogger(string location)
        {
            Type classType;
            try
            {
                var className = location.Split('.').FirstOrDefault();
                var fullyClassName = string.Format("{0}.{1}", this.GetType().Namespace, className);
                classType = Type.GetType(fullyClassName) ?? this.GetType();
            }
            catch
            {
                classType = this.GetType();
            }

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

    internal sealed class AutofacLoggingModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var loggingServiceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var loggingFactory = loggingServiceProvider.GetRequiredService<ILoggerFactory>();
            loggingFactory.AddConsole();
            builder.Register(ctx => loggingFactory);
            builder.RegisterType<FactoryLogger>().As<Logging.ILogger>();
        }
    }
}
