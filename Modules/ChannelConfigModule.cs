﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using RuGatherBot.Common;
using RuGatherBot.Managers;

namespace RuGatherBot.Modules
{
    [Name("config")]
    [Summary("Bot configuration options")]
    public class ChannelConfigModule : GatherModuleBase
    {
        private readonly GatherManager gatherManager;
        
        public ChannelConfigModule(GatherManager gatherManager)
        {
            this.gatherManager = gatherManager;
        }

        [Command("prefix")]
        [Summary("Check what prefix this channel has configured.")]
        public async Task PrefixAsync()
        {
            var config = await gatherManager.GetOrCreateConfigAsync(Context.Channel.Id);
            
            if (config.Prefix == null)
                await ReplyAsync($"This channel's prefix is {Context.Client.CurrentUser.Mention}");
            else
                await ReplyAsync($"This channel's prefix is `{config.Prefix}`");
        }

        [Command("setprefix")]
        [Summary("Change or remove this channel's string prefix.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetPrefixAsync([Remainder]string prefix)
        {
            await gatherManager.SetPrefixAsync(Context.Channel.Id, prefix);

            await ReplyAsync($"This channel's prefix is now `{prefix}`");
        }

        [Command("configstatus")]
        [Summary("Check channel's status.")]
        public async Task StatusAsync()
        {
            var config = await gatherManager.GetOrCreateConfigAsync(Context.Channel.Id);
            var not = config.GatherAllowed ? string.Empty : "not ";
            await ReplyAsync($"Gather is {not} allowed. Team size is {config.TeamSize}.");
        }
        
        [Command("allowgather")]
        [Summary("Allow gather on this channel.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AllowGather()
        {
            var config = await gatherManager.GetOrCreateConfigAsync(Context.Channel.Id);
            config.GatherAllowed = true;
            await gatherManager.UpdateAsync(config);

            await ReplyAsync($"Gather is allowed on this channel now.");
        }
        
        [Command("disallowgather")]
        [Summary("Disallow gather on this channel.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DisallowGather()
        {
            var config = await gatherManager.GetOrCreateConfigAsync(Context.Channel.Id);
            config.GatherAllowed = false;
            await gatherManager.UpdateAsync(config);

            await ReplyAsync($"Gather is not allowed on this channel now.");
        }
        
        [Command("teamsize")]
        [Summary("Check what team size this channel has configured.")]
        public async Task TeamSizeAsync()
        {
            var config = await gatherManager.GetOrCreateConfigAsync(Context.Channel.Id);
            
            await ReplyAsync($"This channel's team size is {config.TeamSize}");
        }

        [Command("setteamsize")]
        [Summary("Change team size for this channel.")]
        [RequireUserPermission(ChannelPermission.ManageChannel)]
        public async Task SetTeamSizeAsync(uint teamsize)
        {
            var config = await gatherManager.GetOrCreateConfigAsync(Context.Channel.Id);
            config.TeamSize = teamsize;
            await gatherManager.UpdateAsync(config);

            await ReplyAsync($"This channel's team size is now {config.TeamSize}");
        }
        
    }
}
