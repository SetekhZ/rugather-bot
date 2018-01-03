using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RuGatherBot.Common;
using RuGatherBot.Managers;

namespace RuGatherBot.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient discord;
        private readonly CommandService commands;
        private readonly LoggingService logger;
        private readonly ChannelConfigManager channelConfigManager;
        private readonly IServiceProvider serviceProvider;

        public CommandHandler(
            DiscordSocketClient discord,
            CommandService commands,
            LoggingService logger,
            ChannelConfigManager channelConfigManager,
            IServiceProvider serviceProvider)
        {
            this.discord = discord;
            this.commands = commands;
            this.logger = logger;
            this.channelConfigManager = channelConfigManager;
            this.serviceProvider = serviceProvider;

            this.discord.MessageReceived += OnMessageReceivedAsync;
        }
        
        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg))
                return;
            var context = new GatherCommandContext(discord, msg);
            var prefix = await channelConfigManager.GetPrefixAsync(context.Channel.Id);

            var argPos = 0;
            var hasStringPrefix = prefix != null && msg.HasStringPrefix(prefix, ref argPos);

            if (hasStringPrefix || msg.HasMentionPrefix(discord.CurrentUser, ref argPos))
                using (context.Channel.EnterTypingState())
                    await ExecuteAsync(context, serviceProvider, argPos);
        }

        public async Task ExecuteAsync(GatherCommandContext context, IServiceProvider provider, int argPos)
        {
            var result = await commands.ExecuteAsync(context, argPos, provider);
            await ResultAsync(context, result);
        }

        public async Task ExecuteAsync(GatherCommandContext context, IServiceProvider provider, string input)
        {
            var result = await commands.ExecuteAsync(context, input, provider);
            await ResultAsync(context, result);
        }

        private async Task ResultAsync(GatherCommandContext context, IResult result)
        {
            if (result.IsSuccess)
                return;
            
            switch (result)
            {
                case ExecuteResult exr:
                    await logger.LogAsync(LogSeverity.Error, "Commands", exr.Exception?.ToString() ?? exr.ErrorReason);
                    break;
                default:
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                    break;
            }
        }
    }
}
