using System.Collections.Generic;

namespace _101mngr.Domain
{
    public class Team
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IReadOnlyList<MatchPlayer> Players { get; set; }
    }
}