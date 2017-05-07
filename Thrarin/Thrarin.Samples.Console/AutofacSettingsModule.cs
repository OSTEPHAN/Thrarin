
namespace Thrarin.Console
{
    using Autofac;
    using Configuration;
    using Microsoft.Extensions.Configuration;
    using System.Linq;

    internal sealed class SettingsProvider : ISettingsProvider
    {
        private readonly IConfigurationRoot configurationRoot;
        public SettingsProvider(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot;

        }
        IQueryable<Setting> ISettingsProvider.Query =>
            configurationRoot
            .AsEnumerable()
            .Select(kvp => new Setting() { Key = kvp.Key, Value = kvp.Value })
            .AsQueryable();
    }

    internal sealed class AutofacSettingsModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            builder.Register(ctx => configurationBuilder);
            builder.RegisterType<SettingsProvider>().As<ISettingsProvider>();
        }
    }
}
