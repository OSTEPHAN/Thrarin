
namespace Thrarin.Console
{
    using Caching;
    using Configuration;
    using Logging;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Storage;
    using System.Linq;

    interface IBlogService
    {
        Post LastPost { get; }
    }

    class BlogService : IBlogService
    {
        private readonly IEntityQuery entityQuery;
        public BlogService(IEntityQuery entityQuery)
        {
            this.entityQuery = entityQuery;
        }

        Post IBlogService.LastPost => this.entityQuery
            .Query<Blog>(b => b.Posts)
            .SelectMany(b => b.Posts)
            .LastOrDefault();
    }

    class Program
    {
        static void Main(string[] args)
        {
            var autofac = new AutofacDependencyResolver() as IDependencyResolver;
            autofac.Register<IBlogService, BlogService>();
            autofac.Configure();

            var entityStore = autofac.Resolve<IEntityStore>();
            entityStore.Install();

            var logger = autofac.Resolve<Logging.ILogger>();
            var cache = autofac.Resolve<Caching.ICacheProvider>();

            logger.LogInformation("Starting ...");
            while (System.Console.ReadKey().Key != System.ConsoleKey.Escape)
            {
                var cacheItem = cache.Get() as Post;

                if (null != cacheItem)
                {
                    logger.LogInformation("Reading Cache");
                    logger.LogInformation(cacheItem.Title);
                    continue;
                }

                logger.LogInformation("Setting Cache");
                cache.Set(autofac.Resolve<IBlogService>().LastPost);
            }
        }
    }
}
