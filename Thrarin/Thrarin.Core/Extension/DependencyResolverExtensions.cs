
namespace Thrarin.Configuration
{
    using System.Linq;
    using System.Reflection;

    public static class DependencyResolverExtensions
    {
        public static void Configure(this IDependencyResolver configurator, params Assembly[] assemblies)
        {
            configurator.Register(configurator.GetType().GetTypeInfo().Assembly);
            if (null != assemblies && assemblies.Any())
            {
                foreach (var a in assemblies)
                {
                    configurator.Register(a);
                }
            };
            configurator.Build();
        }
    }
}
