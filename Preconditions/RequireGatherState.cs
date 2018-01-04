using System;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using RuGatherBot.Entities.Gather;
using RuGatherBot.Managers;

namespace RuGatherBot.Preconditions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class RequireGatherState : PreconditionAttribute
    {
        public GatherState State { get; set; }

        public RequireGatherState(GatherState state)
        {
            State = state;
        }

        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var gatherManager = services.GetRequiredService<GatherManager>();
            var gather = gatherManager.GetGatherInProgressAsync(context.Channel.Id).Result;
            return Task.FromResult((gather?.State ?? GatherState.Join) == State ? PreconditionResult.FromSuccess() : PreconditionResult.FromError(string.Empty));
        }
    }
}