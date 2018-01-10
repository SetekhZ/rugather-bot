using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace RuGatherBot.Entities.Gather
{
    public enum GatherState
    {
        Join,
        Vote,
        SelectTeams,
        Ended
    }
    
    public class Gather
    {
        public Gather()
        {
        }

        public Gather(ulong channelId, uint teamSize)
        {
            ChannelId = channelId;
            TeamSize = teamSize;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }
        [Required]
        public ulong ChannelId { get; set; }
        [Required]
        public uint TeamSize { get; set; }

        [Required]
        public GatherState State { get; set; } = GatherState.Join;
        [Required]
        public DateTime BeginTime { get; set; } = DateTime.Now;
        public DateTime? EndTime { get; set; }
        
        public List<GatherPlayer> Players { get; set; } = new List<GatherPlayer>();

        public bool Join(GatherPlayer player)
        {
            if (Players.Count > TeamSize * 2 || Players.Any(x => x.Id == player.Id))
            {
                return false;
            }
            Players.Add(player);
            CheckStatusChange();
            return true;
        }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Gather started at {BeginTime} with {TeamSize} players in one team.");
            if (State == GatherState.Join)
            {
                sb.AppendLine($"Joined ({GetPlayersCountsString()}): {GetPlayersString()}");
            }

            return sb.ToString();
        }

        public string GetPlayersCountsString()
        {
            return $"{Players?.Count ?? 0}/{TeamSize * 2}";
        }
        
        public string GetPlayersString()
        {
            return Players == null ? string.Empty : string.Join(", ", Players.Select(x => x.UserName));
        }

        private void CheckStatusChange()
        {
            
        }
    }
}