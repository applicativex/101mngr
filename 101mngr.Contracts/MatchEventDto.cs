namespace _101mngr.Contracts
{
    public class MatchEventDto
    {
        public string MatchId { get; set; } 

        public bool? Home { get; set; }

        public string PlayerId { get; set; }

        public MatchEventType MatchEventType { get; set; }

        public string SubstitutionPlayerId { get; set; }

        public int Minute { get; set; }

        public MatchPeriod MatchPeriod { get; set; }    
    }
}