
namespace Thrarin.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Reflection;

    using Configuration;

    internal sealed class ServiceCollectionDependencyResolver : IDependencyResolver
    {
        private IServiceProvider container;
        private ServiceCollection containerBuilder;
        private ServiceCollection ContainerBuilder
        {
            get
            {
                if (null == this.containerBuilder)
                {
                    this.containerBuilder = new ServiceCollection();
                }
                return this.containerBuilder;
            }
        }

        void IDependencyResolver.Build()
        {
            this.container = this.ContainerBuilder.BuildServiceProvider();
        }

        void IDependencyResolver.Register(Assembly assembly)
        {
        }

        void IDependencyResolver.Register<TImplementation>()
        {
            this.ContainerBuilder.AddTransient(typeof(TImplementation));
        }

        void IDependencyResolver.Register<TInterface, TImplementation>()
        {
            this.ContainerBuilder.AddTransient(typeof(TInterface), typeof(TImplementation));
        }

        void IDependencyResolver.Register<T>(T instance)
        {
            this.ContainerBuilder.AddSingleton(typeof(T), instance);
        }

        void IDependencyResolver.Register<T>(Func<T> factory)
        {
            this.ContainerBuilder.AddTransient(typeof(T), serviceProvider => factory());
        }

        T IDependencyResolver.Resolve<T>()
        {
            return (T)this.container.GetRequiredService(typeof(T));
        }
    }
}

