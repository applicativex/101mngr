using System;
using System.Collections.Generic;
using _101mngr.Contracts.Enums;

namespace _101mngr.Contracts.Models
{
    public class MatchDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime StartTime { get; set; } 

        public int Minute { get; set; }

        public MatchPeriod MatchPeriod { get; set; }

        public ScoreDto Goals { get; set; }

        public ScoreDto YellowCards { get; set; }

        public ScoreDto RedCards { get; set; }

        public TeamDto HomeTeam { get; set; }

        public TeamDto AwayTeam { get; set; }

        public List<MatchEventDto> MatchEvents { get; set; }
    }

    public class ScoreDto
    {
        public int Home { get; set; }

        public int Away { get; set; }
    }

    public class TeamDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IReadOnlyList<MatchPlayerDto> Players { get; set; }
    }

    public class MatchPlayerDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool Bench { get; set; }

        public PlayerType PlayerType { get; set; }
    }
}