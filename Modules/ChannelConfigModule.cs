using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using RuGatherBot.Common;
using RuGatherBot.Managers;

namespace RuGatherBot.Modules.Admin
{
    [Name("config")]
    [Summary("Bot configuration options")]
    public class ChannelConfigModule : GatherModuleBase
    {
        private readonly ChannelConfigManager channelConfigManager;
        
        public ChannelConfigModule(ChannelConfigManager channelConfigManager)
        {
            this.channelConfigManager = channelConfigManager;
        }

        [Command("prefix")]
        [Summary("Check what prefix this channel has configured.")]
        public async Task PrefixAsync()
        {
            var config = await channelConfigManager.GetOrCreateConfigAsync(Context.Channel.Id);
            
            if (config.Prefix == null)
                await ReplyAsync($"This channel's prefix is {Context.Client.CurrentUser.Mention}");
            else
                await ReplyAsync($"This channel's prefix is `{config.Prefix}`");
        }

        [Command("setprefix")]
        [Summary("Change or remove this channel's string prefix.")]
        [RequireUserPermission(ChannelPermission.ManageChannel)]
        public async Task SetPrefixAsync([Remainder]string prefix)
        {
            var config = await channelConfigManager.GetOrCreateConfigAsync(Context.Channel.Id);
            await channelConfigManager.SetPrefixAsync(config, prefix);

            await ReplyAsync($"This channel's prefix is now `{prefix}`");
        }

        [Command("status")]
        [Summary("Check channel's status.")]
        public async Task StatusAsync()
        {
            var config = await channelConfigManager.GetOrCreateConfigAsync(Context.Channel.Id);
            var not = config.GatherAllowed ? string.Empty : "not ";
            await ReplyAsync($"Gather is {not} allowed. Team size is {config.TeamSize}.");
        }
        
        [Command("allowgather")]
        [Summary("Allow gather on this channel.")]
        [RequireUserPermission(ChannelPermission.ManageChannel)]
        public async Task AllowGather()
        {
            var config = await channelConfigManager.GetOrCreateConfigAsync(Context.Channel.Id);
            await channelConfigManager.SetGatherAllowedAsync(config, true);

            await ReplyAsync($"Gather is allowed on this channel now.");
        }
        
        [Command("disallowgather")]
        [Summary("Disallow gather on this channel.")]
        [RequireUserPermission(ChannelPermission.ManageChannel)]
        public async Task DisallowGather()
        {
            var config = await channelConfigManager.GetOrCreateConfigAsync(Context.Channel.Id);
            await channelConfigManager.SetGatherAllowedAsync(config, false);

            await ReplyAsync($"Gather is not allowed on this channel now.");
        }
        
        [Command("teamsize")]
        [Summary("Check what team size this channel has configured.")]
        public async Task TeamSizeAsync()
        {
            var config = await channelConfigManager.GetOrCreateConfigAsync(Context.Channel.Id);
            
            await ReplyAsync($"This channel's team size is {config.TeamSize}");
        }

        [Command("setteamsize")]
        [Summary("Change team size for this channel.")]
        [RequireUserPermission(ChannelPermission.ManageChannel)]
        public async Task SetTeamSizeAsync(uint teamsize)
        {
            var config = await channelConfigManager.GetOrCreateConfigAsync(Context.Channel.Id);
            await channelConfigManager.SetTeamSizeAsync(config, teamsize);

            await ReplyAsync($"This channel's team size is now {config.TeamSize}");
        }
        
    }
}
