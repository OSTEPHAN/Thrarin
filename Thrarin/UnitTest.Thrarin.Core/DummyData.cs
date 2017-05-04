
namespace Thrarin.Tests
{
    using Caching;
    using Logging;
    using Storage;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal sealed class DummyData : IEntity
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
    }

    internal sealed class DummyContext : MemoryContext
    {
        private readonly ILogger logger;
        private readonly static Dictionary<Type, IEnumerable> Repository = new Dictionary<Type, IEnumerable>() { };

        public DummyContext(ILogger logger) : base(DummyContext.Repository)
        {
            this.logger = logger;
        }
    }

    internal sealed class DummyCacheProvider : MemoryCacheProvider
    {
        public List<Tuple<string, object, DateTimeOffset>> Cache => DummyCacheProvider.cache;
        private readonly static List<Tuple<string, object, DateTimeOffset>> cache = new List<Tuple<string, object, DateTimeOffset>>();
        public DummyCacheProvider() : base(DummyCacheProvider.cache)
        {
        }
    }
}
