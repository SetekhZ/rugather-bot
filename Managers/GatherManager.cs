using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RuGatherBot.Contracts;
using RuGatherBot.Databases;
using RuGatherBot.Entities;
using RuGatherBot.Entities.Gather;

namespace RuGatherBot.Managers
{
    public class GatherManager : DbManager<GatherDatabase>
    {
        public GatherManager(GatherDatabase db) : base(db)
        {
        }

        #region ChannelConfig 

        public Task<ChannelConfig> GetConfigAsync(ulong channelId)
            => db.ChannelConfigs.SingleOrDefaultAsync(x => x.ChannelId == channelId);

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
            => (await GetConfigAsync(channelId))?.Prefix;

        public async Task SetPrefixAsync(ulong channelId, string prefix)
        {
            var config = await GetConfigAsync(channelId);
            if (config != null)
            {
                config.Prefix = string.IsNullOrWhiteSpace(prefix) ? null : prefix;

                await UpdateAsync(config);
            }
            else
            {
                config = new ChannelConfig(channelId)
                {
                    Prefix = string.IsNullOrWhiteSpace(prefix) ? null : prefix
                };
                await CreateAsync(config);
            }
        }

        public async Task UpdateAsync(ChannelConfig config)
        {
            db.ChannelConfigs.Update(config);
            await db.SaveChangesAsync();
        }
        
        public async Task CreateAsync(ChannelConfig channelConfig)
        {
            await db.ChannelConfigs.AddAsync(channelConfig);
            await db.SaveChangesAsync();
        }
        
        #endregion

        #region Gather
        
        public Task<Gather> GetGatherInProgressAsync(ulong channelId)
            => db.Gathers.Include(x => x.Players).SingleOrDefaultAsync(x => x.ChannelId == channelId && x.State != GatherState.Ended);

        public async Task<Gather> GetGatherInProgressOrCreateAsync(ulong channelId)
        {
            var gather = await GetGatherInProgressAsync(channelId);
            if (gather != null)
            {
                return gather;
            }
            
            var config = await GetConfigAsync(channelId) ?? new ChannelConfig();
            gather = new Gather(channelId, config.TeamSize);
            await CreateAsync(gather);
            return await GetGatherInProgressAsync(channelId);
        }
        
        public async Task UpdateAsync(Gather gather)
        {
            db.Gathers.Update(gather);
            await db.SaveChangesAsync();
        }
        
        public async Task CreateAsync(Gather gather)
        {
            await db.Gathers.AddAsync(gather);
            await db.SaveChangesAsync();
        }
        #endregion
    }
}