using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuGatherBot.Entities.Gather
{
    public class GatherPlayer
    {
        public GatherPlayer()
        {
        }

        public GatherPlayer(ulong userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }
        [Required]
        public ulong UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public uint TeamNumber { get; set; } = 0;
        
        public Gather Gather { get; set; }
    }
}