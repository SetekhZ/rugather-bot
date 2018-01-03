using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RuGatherBot.Contracts;
using RuGatherBot.Databases;
using RuGatherBot.Entities;

namespace RuGatherBot.Managers
{
    public class ConfigManager : DbManager<ConfigDatabase>
    {
        public ConfigManager(ConfigDatabase db)
            : base(db) { }

        public Task<GuildConfig> GetConfigAsync(ulong guildId)
            => _db.GuildConfigs.SingleOrDefaultAsync(x => x.GuildId == guildId);

        public async Task<GuildConfig> GetOrCreateConfigAsync(ulong guildId)
        {
            var config = await GetConfigAsync(guildId);
            if (config != null)
                return config;

            await CreateAsync(new GuildConfig
            {
                GuildId = guildId
            });

            return await GetConfigAsync(guildId);
        }

        public async Task<string> GetPrefixAsync(ulong guildId)
            => (await GetOrCreateConfigAsync(guildId)).Prefix;

        public async Task SetPrefixAsync(ulong guildId, string prefix)
        {
            var config = await GetConfigAsync(guildId);
            await SetPrefixAsync(config, prefix);
        }

        public async Task SetPrefixAsync(GuildConfig config, string prefix)
        {
            config.Prefix = string.IsNullOrWhiteSpace(prefix) ? null : prefix;

            _db.GuildConfigs.Update(config);
            await _db.SaveChangesAsync();
        }

        public async Task CreateAsync(GuildConfig guildConfig)
        {
            await _db.GuildConfigs.AddAsync(guildConfig);
            await _db.SaveChangesAsync();
        }
    }
}
