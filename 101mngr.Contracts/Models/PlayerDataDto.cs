using _101mngr.Contracts.Enums;

namespace _101mngr.Contracts.Models
{
    public class PlayerDataDto
    {
        public long? Id { get; set; }
        public bool IsCaptain { get; set; } 
        public string UserName { get; set; }
        public int Level { get; set; }
        public PlayerType PlayerType { get; set; }
        public bool IsVirtual { get; set; } 
    }
}