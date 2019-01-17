namespace _101mngr.WebApp.Domain
{
    public class FootballLeagueSeason
    {
        public string Id { get; set; }

        public string FootballLeagueId { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public string[] FootballClubIds { get; set; }
    }
}