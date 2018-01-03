using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using RuGatherBot.Entities;

namespace RuGatherBot.Databases
{
    public sealed class ChannelConfigDatabase : DbContext
    {
        public DbSet<ChannelConfig> ChannelConfigs { get; set; }

        public ChannelConfigDatabase()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var baseDir = Path.Combine(AppContext.BaseDirectory, "data");
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            var datadir = Path.Combine(baseDir, "channelconfig.sqlite.db");
            optionsBuilder.UseSqlite($"Filename={datadir}");
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ChannelConfig>()
                .HasKey(x => x.Id);
            builder.Entity<ChannelConfig>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Entity<ChannelConfig>()
                .Property(x => x.ChannelId)
                .IsRequired();
        }
    }
}
