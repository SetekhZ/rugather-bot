using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using RuGatherBot.Common;
using RuGatherBot.Managers;

namespace RuGatherBot.Modules.Admin
{
    [Name("Config")]
    [Summary("Bot configuration options")]
    public class GuildModule : GatherModuleBase
    {
        private readonly ConfigManager configManager;
        
        public GuildModule(ConfigManager configManager)
        {
            this.configManager = configManager;
        }

        [Command("prefix")]
        [Summary("Check what prefix this guild has configured.")]
        public async Task PrefixAsync()
        {
            var config = await configManager.GetOrCreateConfigAsync(Context.Guild.Id);

            if (config.Prefix == null)
                await ReplyAsync($"This guild's prefix is {Context.Client.CurrentUser.Mention}");
            else
                await ReplyAsync($"This guild's prefix is `{config.Prefix}`");
        }

        [Command("setprefix")]
        [Summary("Change or remove this guild's string prefix.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetPrefixAsync([Remainder]string prefix)
        {
            var config = await configManager.GetOrCreateConfigAsync(Context.Guild.Id);
            await configManager.SetPrefixAsync(config, prefix);

            await ReplyAsync($"This guild's prefix is now `{prefix}`");
        }
    }
}
