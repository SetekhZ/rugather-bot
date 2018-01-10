using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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
        {
            var webHost = BuildWebHost(args);
            webHost.Services.GetRequiredService<LoggingService>();
            webHost.Services.GetRequiredService<StartupService>().StartAsync().Wait();
            webHost.Services.GetRequiredService<CommandHandler>();
            webHost.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}