using System;

namespace _101mngr.WebApp.Data
{
    public class PlayerMatchHistory
    {
        public Guid Id { get; set; }

        public long PlayerId { get; set; }

        public Player Player { get; set; }  

        public long MatchId { get; set; }

        public Match Match { get; set; }    
    }
}