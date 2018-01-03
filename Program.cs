using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RuGatherBot.Databases;
using RuGatherBot.Managers;
using RuGatherBot.Services;

namespace RuGatherBot
{
    public class Program
    {
        public static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();

        private IConfigurationRoot config;

        private async Task StartAsync()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("configuration.json");
            config = builder.Build();

          
            var services = new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Verbose,
                    MessageCacheSize = 100
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    DefaultRunMode = RunMode.Async,
                    LogLevel = LogSeverity.Verbose
                }))
                .AddDbContext<ConfigDatabase>(ServiceLifetime.Transient)
                .AddTransient<ConfigManager>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<LoggingService>()
                .AddSingleton<StartupService>()
                .AddSingleton<Random>()
                .AddSingleton(config);

            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<LoggingService>();

            await provider.GetRequiredService<StartupService>().StartAsync();

            provider.GetRequiredService<CommandHandler>();

            await Task.Delay(-1);
        }    }
}