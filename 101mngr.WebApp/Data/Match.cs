using System;

namespace _101mngr.WebApp.Data
{
    public class Match
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int TournamentId { get; set; }   

        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public DateTime StartDate { get; set; }

        public string Scoreboard { get; set; }  
    }
}