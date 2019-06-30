namespace _101mngr.Contracts.Models
{
    public class MatchEventDto
    {
        public string Id { get; set; }  

        public string MatchId { get; set; } 

        public bool? Home { get; set; }

        public string PlayerId { get; set; }

        public int MatchEventType { get; set; }

        public string SubstitutionPlayerId { get; set; }

        public int Minute { get; set; }

        public int MatchPeriod { get; set; }    
    }
}