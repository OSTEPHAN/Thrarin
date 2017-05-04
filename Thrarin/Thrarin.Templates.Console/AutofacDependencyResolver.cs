
namespace Thrarin.Console
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Reflection;

    internal class AutofacDependencyResolver : IDependencyResolver
    {
        private IServiceProvider container;
        private ContainerBuilder containerBuilder;
        private ContainerBuilder ContainerBuilder
        {
            get
            {
                if (null == this.containerBuilder)
                {
                    this.containerBuilder = new ContainerBuilder();
                }
                return this.containerBuilder;
            }
        }

        void IDependencyResolver.Build()
        {
            this.container = new AutofacServiceProvider(this.ContainerBuilder.Build());
        }

        void IDependencyResolver.Register(Assembly assembly)
        {
            this.ContainerBuilder.RegisterAssemblyModules(assembly);
        }

        void IDependencyResolver.Register<TImplementation>()
        {
            this.ContainerBuilder.RegisterType<TImplementation>();
        }

        void IDependencyResolver.Register<TInterface, TImplementation>()
        {
            this.ContainerBuilder.RegisterType<TImplementation>().As<TInterface>();
        }

        void IDependencyResolver.Register<T>(T instance)
        {
            var registerInstanceMethod = typeof(Autofac.RegistrationExtensions).GetMethods().FirstOrDefault(m => m.Name == "RegisterInstance");
            registerInstanceMethod = registerInstanceMethod.MakeGenericMethod(new Type[] { typeof(T) });
            registerInstanceMethod.Invoke(null, new object[] { this.ContainerBuilder, instance });
        }

        void IDependencyResolver.Register<T>(Func<T> factory)
        {
            this.ContainerBuilder.Register<T>(ctx => factory());
        }

        T IDependencyResolver.Resolve<T>()
        {
            return (T)this.container.GetRequiredService(typeof(T));
        }
    }
}

