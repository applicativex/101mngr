using _101mngr.Domain.Enums;

namespace _101mngr.Domain
{
    public class MatchPlayer
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool? Home { get; set; }

        public bool Bench { get; set; }

        public PlayerType PlayerType { get; set; }

        public bool IsVirtual { get; set; }
    }
}