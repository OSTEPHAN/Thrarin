
namespace Thrarin.Console
{
    using Configuration;
    using Storage;

    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");

            var autofac = new AutofacDependencyResolver() as IDependencyResolver;
            autofac.Configure();

            var entityStore = autofac.Resolve<IEntityStore>();
            entityStore.Install();

            System.Console.ReadLine();
        }
    }
}
