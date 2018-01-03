using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RuGatherBot.Contracts;
using RuGatherBot.Databases;
using RuGatherBot.Entities;

namespace RuGatherBot.Managers
{
    public class ChannelConfigManager : DbManager<ChannelConfigDatabase>
    {
        public ChannelConfigManager(ChannelConfigDatabase db)
            : base(db) { }

        public Task<ChannelConfig> GetConfigAsync(ulong channelId)
            => _db.ChannelConfigs.SingleOrDefaultAsync(x => x.ChannelId == channelId);

        public async Task<ChannelConfig> GetOrCreateConfigAsync(ulong channelId)
        {
            var config = await GetConfigAsync(channelId);
            if (config != null)
                return config;

            await CreateAsync(new ChannelConfig
            {
                ChannelId = channelId
            });

            return await GetConfigAsync(channelId);
        }

        public async Task<string> GetPrefixAsync(ulong channelId)
            => (await GetOrCreateConfigAsync(channelId)).Prefix;

        public async Task SetPrefixAsync(ChannelConfig config, string prefix)
        {
            config.Prefix = string.IsNullOrWhiteSpace(prefix) ? null : prefix;

            _db.ChannelConfigs.Update(config);
            await _db.SaveChangesAsync();
        }
        
        public async Task SetGatherAllowedAsync(ChannelConfig config, bool allow)
        {
            config.GatherAllowed = allow;

            _db.ChannelConfigs.Update(config);
            await _db.SaveChangesAsync();
        }

        public async Task SetTeamSizeAsync(ChannelConfig config, uint teamSize)
        {
            config.TeamSize = teamSize;

            _db.ChannelConfigs.Update(config);
            await _db.SaveChangesAsync();
        }
        
        
        public async Task CreateAsync(ChannelConfig channelConfig)
        {
            await _db.ChannelConfigs.AddAsync(channelConfig);
            await _db.SaveChangesAsync();
        }
    }
}
