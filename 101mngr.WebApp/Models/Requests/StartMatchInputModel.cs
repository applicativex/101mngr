namespace _101mngr.WebApp.Controllers
{
    public class StartMatchInputModel
    {
        public int MatchId { get; set; }

        public long[] HomeTeamPlayers { get; set; }

        public long[] AwayTeamPlayers { get; set; }
    }
}