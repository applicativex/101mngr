namespace _101mngr.WebApp.Domain
{
    public class FootballTeam
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public FootballPlayer[] Players { get; set; }
    }
}