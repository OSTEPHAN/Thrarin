
namespace Thrarin.Console
{
    using Configuration;
    using Logging;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Storage;

    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");

            var autofac = new AutofacDependencyResolver() as IDependencyResolver;
            var loggingServiceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var loggingFactory = loggingServiceProvider.GetRequiredService<ILoggerFactory>();
            loggingFactory.AddConsole();
            autofac.Register(() => loggingFactory);
            autofac.Configure();

            var entityStore = autofac.Resolve<IEntityStore>();
            entityStore.Install();

            autofac.Resolve<Logging.ILogger>().LogInformation("Starting ...");
            System.Console.ReadLine();
        }
    }
}
