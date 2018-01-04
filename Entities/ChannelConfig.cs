using System.ComponentModel.DataAnnotations;

namespace RuGatherBot.Entities
{
    public class ChannelConfig
    {
        public ChannelConfig()
        {
        }

        public ChannelConfig(ulong channelId)
        {
            ChannelId = channelId;
        }

        [Key]
        public ulong ChannelId { get; set; }
        public string Prefix { get; set; }
        [Required]
        public bool GatherAllowed { get; set; }
        [Required]
        public uint TeamSize { get; set; } = 6;
    }
}
