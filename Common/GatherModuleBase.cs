﻿using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace RuGatherBot.Common
{
    public class GatherModuleBase : ModuleBase<GatherCommandContext>
    {
        public async Task<IUserMessage> ReplyAsync(Embed embed = null, RequestOptions options = null)
        {
            return await Context.Channel.SendMessageAsync("", false, embed, options).ConfigureAwait(false);
        }

        public async Task<IUserMessage> ReplyAsync(string message, Embed embed = null, RequestOptions options = null)
        {
            return await Context.Channel.SendMessageAsync(message, false, embed, options).ConfigureAwait(false);
        }
        
        public async Task<IUserMessage> ReplyAsync(Stream stream, string fileName, string message = null, RequestOptions options = null)
        {
            return await Context.Channel.SendFileAsync(stream, fileName, message, false, options).ConfigureAwait(false);
        }
    }
}