namespace _101mngr.WebApp.Domain
{
    /// <summary>
    /// Static description of soccer league e.g. Premier League, Primera, Bundesliga
    /// </summary>
    public class FootballLeague
    {
        public string Id { get; set; }

        public string CurrentSeasonId { get; set; } 

        public string Name { get; set; }

        public string CountryId { get; set; }

        public int Level { get; set; }
    }
}