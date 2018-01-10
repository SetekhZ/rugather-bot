using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using RuGatherBot.Common;
using RuGatherBot.Entities.Gather;
using RuGatherBot.Managers;
using RuGatherBot.Preconditions;

namespace RuGatherBot.Modules
{
    [Name("gather")]
    [Summary("Gather")]
    public class GatherModule : GatherModuleBase
    {
        private readonly GatherManager gatherManager;

        public GatherModule(GatherManager gatherManager)
        {
            this.gatherManager = gatherManager;
        }
        
        [Command("status")]
        [Summary("Check gather's status.")]
        [RequireAllowedGathers]
        public async Task StatusAsync()
        {
            var gather = await gatherManager.GetGatherInProgressAsync(Context.Channel.Id);
            await ReplyAsync(gather == null ? "No gather in progress." : gather.ToString());
        }

        [Command("join")]
        [Summary("Join current gather")]
        [RequireAllowedGathers]
        [RequireGatherState(GatherState.Join)]
        public async Task JoinAsync()
        {
            var gather = await gatherManager.GetGatherInProgressOrCreateAsync(Context.Channel.Id);
            gather.Join(new GatherPlayer(Context.User.Id, Context.User.Username));
            await gatherManager.UpdateAsync(gather);
            gather = await gatherManager.GetGatherInProgressAsync(Context.Channel.Id);
            await ReplyAsync($"Joined ({gather.GetPlayersCountsString()}): {gather.GetPlayersString()}" );
        }
    }
}