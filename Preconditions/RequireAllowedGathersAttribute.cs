using System;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using RuGatherBot.Managers;

namespace RuGatherBot.Preconditions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class RequireAllowedGathersAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var gatherManager = services.GetRequiredService<GatherManager>();
            var config = gatherManager.GetConfigAsync(context.Channel.Id).Result;
            return Task.FromResult(config != null && config.GatherAllowed ? PreconditionResult.FromSuccess() : PreconditionResult.FromError(string.Empty)); 
        }
    }
}