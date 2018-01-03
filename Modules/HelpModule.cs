using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using RuGatherBot.Common;
using RuGatherBot.Managers;

namespace RuGatherBot.Modules
{
    [Group("help"), Name("Help")]
    public class HelpModule : GatherModuleBase
    {
        private readonly IServiceProvider provider;
        private readonly ChannelConfigManager channelConfigManager;
        private readonly CommandService commandService;

        public HelpModule(
            CommandService commandService,
            ChannelConfigManager channelConfigManager, 
            IServiceProvider provider)
        {
            this.provider = provider;
            this.channelConfigManager = channelConfigManager;
            this.commandService = commandService;
        }

        private async Task<string> GetPrefixAsync()
            => (await channelConfigManager.GetPrefixAsync(Context.Channel.Id)) ?? $"@{Context.Client.CurrentUser.Username} ";

        [Command]
        public async Task HelpAsync()
        {
            var prefix = await GetPrefixAsync();
            var modules = commandService.Modules.Where(x => !string.IsNullOrWhiteSpace(x.Summary));

            var builder = new EmbedBuilder()
                .WithFooter(x => x.Text = $"Type `{prefix}help <module>` for more information");

            foreach (var module in modules)
            {
                var success = false;
                foreach (var command in module.Commands)
                {
                    var result = await command.CheckPreconditionsAsync(Context, provider);
                    if (!result.IsSuccess) continue;
                    success = true;
                    break;
                }

                if (!success) continue;

                builder.AddField(module.Name, module.Summary);
            }

            await ReplyAsync(builder);
        }

        [Command]
        public async Task HelpAsync(string moduleName)
        {
            var prefix = await GetPrefixAsync();
            var module = commandService.Modules.FirstOrDefault(x => x.Name.ToLower() == moduleName.ToLower());

            if (module == null)
            {
                await ReplyAsync($"The module `{moduleName}` does not exist.");
                return;
            }

            var commands = module.Commands.Where(x => !string.IsNullOrWhiteSpace(x.Summary))
                                 .GroupBy(x => x.Name)
                                 .Select(x => x.First()).ToList();

            if (!commands.Any())
            {
                await ReplyAsync($"The module `{module.Name}` has no available commands :(");
                return;
            }

            var builder = new EmbedBuilder()
                .WithFooter(x => x.Text = $"Type `{prefix}help <command>` for more information");

            foreach (var command in commands)
            {
                var result = await command.CheckPreconditionsAsync(Context, provider);
                if (result.IsSuccess)
                    builder.AddField(prefix + command.Aliases.First(), command.Summary);
            }

            await ReplyAsync(builder);
        }

        [Command]
        public async Task HelpAsync(string moduleName, string commandName)
        {
            var alias = $"{moduleName} {commandName}".ToLower();
            var prefix = await GetPrefixAsync();
            var module = commandService.Modules.FirstOrDefault(x => x.Name.ToLower() == moduleName.ToLower());

            if (module == null)
            {
                await ReplyAsync($"The module `{moduleName}` does not exist.");
                return;
            }

            var commands = module.Commands.Where(x => !string.IsNullOrWhiteSpace(x.Summary)).ToList();

            if (!commands.Any())
            {
                await ReplyAsync($"The module `{module.Name}` has no available commands :(");
                return;
            }

            var command = commands.Where(x => x.Aliases.Contains(alias));
            var builder = new EmbedBuilder();

            var aliases = new List<string>();
            foreach (var overload in command)
            {
                var result = await overload.CheckPreconditionsAsync(Context, provider);
                if (result.IsSuccess)
                {
                    var sbuilder = new StringBuilder()
                        .Append(prefix + overload.Aliases.First());

                    foreach (var parameter in overload.Parameters)
                    {
                        var p = parameter.Name;

                        if (parameter.IsRemainder)
                            p += "...";
                        
                        p = parameter.IsOptional ? $"[{p}]" : $"<{p}>";

                        sbuilder.Append(" " + p);
                    }

                    builder.AddField(sbuilder.ToString(), overload.Remarks ?? overload.Summary);
                }
                aliases.AddRange(overload.Aliases);
            }

            builder.WithFooter(x => x.Text = $"Aliases: {string.Join(", ", aliases)}");

            await ReplyAsync(builder);
        }
    }
}
