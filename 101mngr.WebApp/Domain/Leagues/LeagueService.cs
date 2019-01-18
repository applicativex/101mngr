using System.Linq;

namespace _101mngr.WebApp.Domain
{
    public class LeagueService
    {
        private readonly LeagueDbContext _dbContext;

        public LeagueService(LeagueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Country[] GetCountries()
        {
            return _dbContext.Countries;
        }

        public FootballLeague[] GetLeagues(string countryId)
        {
            return _dbContext.Leagues.Values.Where(x=>x.CountryId == countryId).ToArray();
        }

        public FootballLeague GetLeague(string leagueId)
        {
            return _dbContext.Leagues[leagueId];
        }

        public FootballLeagueSeason[] GetLeagueSeasons(string leagueId)
        {
            return _dbContext.Seasons.Values.Where(x => x.LeagueId == leagueId).ToArray();
        }

        public FootballTeam[] GetSeasonTeams(string seasonId)
        {
            return _dbContext.Seasons[seasonId].TeamIds.Select(x => _dbContext.Teams[x]).ToArray();
        }

        public FootballPlayer[] GetPlayers() => _dbContext.Players.Values.ToArray();

        public FootballPlayer[] GetTeamPlayers(string teamId)
        {
            return _dbContext.Teams[teamId].Players;
        }
    }
}