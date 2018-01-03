namespace RuGatherBot.Entities
{
    public class ChannelConfig
    {
        public ulong Id { get; set; }
        public ulong ChannelId { get; set; }
        public string Prefix { get; set; }
        public bool GatherAllowed { get; set; }
        public uint TeamSize { get; set; } = 6;
    }
}
