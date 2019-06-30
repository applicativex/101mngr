namespace _101mngr.Contracts.Models
{
    public class MatchRoomDto
    {
        public string MatchId { get; set; }

        public string OwnerPlayerId { get; set; }

        public MatchPlayerDto[] Players { get; set; }   

        public MatchPlayerDto[] VirtualPlayers { get; set; }

        public bool MatchStarted { get; set; }  
    }
}