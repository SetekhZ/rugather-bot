using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace RuGatherBot.Services
{
    public class StartupService
    {
        private readonly DiscordSocketClient discord;
        private readonly CommandService commandService;
        private readonly IConfigurationRoot config;
        
        public StartupService(
            DiscordSocketClient discord,
            CommandService commandService,
            IConfigurationRoot config)
        {
            this.config = config;
            this.discord = discord;
            this.commandService = commandService;
        }
        
        public async Task StartAsync()
        {
            await discord.LoginAsync(TokenType.Bot, config["tokens:discord"]);
            await discord.StartAsync();

            commandService.AddTypeReader<Uri>(new UriTypeReader());
            
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly());
        }

    }
}
