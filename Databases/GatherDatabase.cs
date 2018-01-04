using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using RuGatherBot.Entities;
using RuGatherBot.Entities.Gather;

namespace RuGatherBot.Databases
{
    public sealed class GatherDatabase : DbContext
    {
        public DbSet<ChannelConfig> ChannelConfigs { get; set; }
        public DbSet<Gather> Gathers { get; set; }

        public GatherDatabase()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var baseDir = Path.Combine(AppContext.BaseDirectory, "data");
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            var datadir = Path.Combine(baseDir, "gathers.sqlite.db");
            optionsBuilder.UseSqlite($"Filename={datadir}");
        }
    }
}
