using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using RuGatherBot.Common;

namespace RuGatherBot.Modules.Gather
{
    [RequireOwner]
    [Name("GatherAdmin"), Group("gather"), Alias("gatheradmin", "gadmin", "admin")]
    public class GatherAdminModule : GatherModuleBase
    {
        [Command("teamsize")]
        [Summary("Set team size ")]
        public async Task TeamSizeAsync(SocketUser user, int size)
        {
            await Task.Delay(0);
        }

    }
}
