using System;

namespace _101mngr.Contracts.Models
{
    public class MatchListItemDto
    {
        public string Id { get; set; }  

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public ScoreDto Goals { get; set; }

        public int Minute { get; set; }

        public int MatchPeriod { get; set; }

        public TeamDto HomeTeam { get; set; }

        public TeamDto AwayTeam { get; set; }

        public MatchListEventType MatchListEventType { get; set; }  
    }

    public enum MatchListEventType
    {
        None = 0,
        MatchAdded = 1,
        MatchTime,
        MatchGoal,
        MatchRemoved
    }
}   