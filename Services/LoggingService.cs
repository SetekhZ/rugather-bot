using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Rest;

namespace RuGatherBot.Services
{
    public class LoggingService
    {
        private string LogDirectory { get; }
        private string LogFile => Path.Combine(LogDirectory, $"{DateTime.UtcNow:yyyy-MM-dd}.txt");
        
        public LoggingService(DiscordSocketClient discord, CommandService commands)
        {
            LogDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

            discord.Log += OnLogAsync;
            commands.Log += OnLogAsync;
        }

        public Task LogAsync(object severity, string source, string message)
        {
            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);
            if (!File.Exists(LogFile))
                File.Create(LogFile).Dispose();

            var logText = $"{DateTime.UtcNow:hh:mm:ss} [{severity}] {source}: {message}";
            File.AppendAllText(LogFile, logText + "\n");
            Console.WriteLine(logText);
            
            return Task.CompletedTask;
        }

        private Task OnLogAsync(LogMessage msg)
            => LogAsync(msg.Severity, msg.Source, msg.Exception?.ToString() ?? msg.Message);
    }
}
