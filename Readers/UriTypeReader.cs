﻿using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace RuGatherBot.Services
{
    public class UriTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> Read(ICommandContext context, string input, IServiceProvider services)
        {
            if (Uri.TryCreate(input, UriKind.Absolute, out var uri))
                return Task.FromResult(TypeReaderResult.FromSuccess(uri));
            
            return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, $"`{input}` is not a valid url."));
        }
    }
}