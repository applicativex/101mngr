using System;

namespace _101mngr.Contracts.Models
{
    public class MatchInfoDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public PlayerDataDto[] Players { get; set; }    
    }

    public class MatchListItemDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int PlayersCount { get; set; }   
    }
}