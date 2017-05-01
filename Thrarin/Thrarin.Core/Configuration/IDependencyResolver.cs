
namespace Thrarin.Configuration
{
    using System;
    using System.Reflection;

    public interface IDependencyResolver
    {
        void Build();
        void Register(Assembly assembly);
        void Register<TImplementation>();
        void Register<TInterface, TImplementation>();
        void Register<T>(T instance);
        void Register<T>(Func<T> factory);
        T Resolve<T>();
    }
}
