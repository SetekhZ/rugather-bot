﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace RuGatherBot.Common
{
    public class GatherCommandContext : ICommandContext
    {
        public DiscordSocketClient Client { get; }
        public SocketGuild Guild { get; }
        public ISocketMessageChannel Channel { get; }
        public SocketUser User { get; }
        public SocketUserMessage Message { get; }

        public bool IsPrivate => Channel is IPrivateChannel;

        public GatherCommandContext(DiscordSocketClient client, SocketUserMessage msg, SocketUser user = null)
        {
            Client = client;
            Guild = (msg.Channel as SocketGuildChannel)?.Guild;
            Channel = msg.Channel;
            User = user ?? msg.Author;
            Message = msg;
        }

        //ICommandContext
        IDiscordClient ICommandContext.Client => Client;
        IGuild ICommandContext.Guild => Guild;
        IMessageChannel ICommandContext.Channel => Channel;
        IUser ICommandContext.User => User;
        IUserMessage ICommandContext.Message => Message;
        
    }
}