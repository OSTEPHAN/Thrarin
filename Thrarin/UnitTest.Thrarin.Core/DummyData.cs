using System;

namespace Thrarin.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    using Caching;
    using Logging;
    using Storage;

    internal class DummyData : IEntity
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
    }

    internal class DummyContext : MemoryContext
    {
        private readonly ILogger logger;
        private readonly static Dictionary<Type, IEnumerable> Repository = new Dictionary<Type, IEnumerable>() { };

        public DummyContext(ILogger logger) : base(DummyContext.Repository)
        {
            this.logger = logger;
        }
    }

    internal class DummyCacheProvider : MemoryCacheProvider
    {
        public List<Tuple<string, object, DateTimeOffset>> Cache => DummyCacheProvider.cache;
        private readonly static List<Tuple<string, object, DateTimeOffset>> cache = new List<Tuple<string, object, DateTimeOffset>>();
        public DummyCacheProvider() : base(DummyCacheProvider.cache)
        {
        }
    }
}
