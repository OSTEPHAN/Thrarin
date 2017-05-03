
namespace Thrarin.Storage
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Reflection;

    internal static class DbContextExtensions
    {
        internal static void InstallContext(this DbContext dbContext, string plateform, string environment)
        {
            dbContext.Database.EnsureCreated();

            var dbContextType = dbContext.GetType();
            var dbContextAssembly = dbContextType.GetTypeInfo().Assembly;
            var dbContextAssemblyTypes = dbContextAssembly.GetTypes();

            Func<Type, bool> isBaseTypeGenericType = t => t.GetTypeInfo().BaseType?.GetTypeInfo().IsGenericType ?? false;
            Func<Type, Type> baseTypeAsGenericType = t => t.GetTypeInfo().BaseType?.GetGenericTypeDefinition() ?? null;
            var seederTypes = dbContextAssemblyTypes.Where(t => 
                isBaseTypeGenericType(t) && 
                baseTypeAsGenericType(t) == typeof(EntityFrameworkSeed<>));
            var seederType = seederTypes.FirstOrDefault(t => t.Name.ToLower() == plateform.ToLower()) ?? seederTypes.First();

            var seeder = Activator.CreateInstance(seederType, new object[] { dbContext });
            var seederMethod = seeder.GetType().GetMethods().FirstOrDefault(m => m.Name == "Seed");
            seederMethod.Invoke(seeder, new[] { environment.ToLower() });
        }
    }
}
