﻿namespace RuGatherBot.Entities
{
    public class GuildConfig
    {
        public ulong Id { get; set; }
        public ulong GuildId { get; set; }
        public string Prefix { get; set; } = "!";
    }
}
