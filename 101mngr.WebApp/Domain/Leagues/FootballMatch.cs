using System;

namespace _101mngr.WebApp.Domain
{
    public class FootballMatch
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string HomeTeamId { get; set; }

        public string AwayTeamId { get; set; }

        public DateTime StartDate { get; set; }

        public string FootballLeagueSeasonId { get; set; }
    }
}