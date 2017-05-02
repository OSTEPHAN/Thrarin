using System;
using System.Collections.Generic;
using System.Text;

namespace Thrarin.Tests
{
    using System.Collections;

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
}
