
using System;
using Microsoft.EntityFrameworkCore;

namespace Thrarin.Tests
{
    internal sealed class SettingSqlMapping : Storage.EntityFrameworkConfiguration<Configuration.Setting>
    {
        public SettingSqlMapping() : base()
        {
        }
        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Configuration.Setting>()
                .HasKey(s => s.Key);
        }
    }
}
