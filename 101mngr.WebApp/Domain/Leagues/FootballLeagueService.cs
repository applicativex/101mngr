using System.Collections.Generic;
using System.Linq;

namespace _101mngr.WebApp.Domain
{
    public class FootballLeagueService
    {
        private const string EnglandId = "1";
        private const string FranceId = "2";
        private const string BelgiumId = "3";

        private const string PremierLeagueId = "1";
        private const string PremierLeague2019Id = "1";
        private const string ManchesterCityId = "1";
        private const string ManchesterUnitedId = "2";

        private const string DeBruyneId = "1";
        private const string VincentKompanyId = "2";
        private const string MarouaneFellainiId = "3";
        private const string RomeluLukakuId = "4";

        private readonly Dictionary<string, FootballPlayer> _footballPlayers;

        private readonly Dictionary<string, FootballTeam> _footballTeams;

        public FootballLeagueService()
        {
            _footballPlayers = new Dictionary<string, FootballPlayer>
            {
                [DeBruyneId] = new FootballPlayer
                {
                    Id = DeBruyneId,
                    Age = 27,
                    CountryId = BelgiumId,
                    FirstName = "Kevin",
                    LastName = "De Bruyne"
                },
                [VincentKompanyId] = new FootballPlayer
                {
                    Id = VincentKompanyId,
                    Age = 32,
                    CountryId = BelgiumId,
                    FirstName = "Vincent",
                    LastName = "Kompany"
                },
                [MarouaneFellainiId] = new FootballPlayer
                {
                    Id = MarouaneFellainiId,
                    Age = 31,
                    CountryId = BelgiumId,
                    FirstName = "Marouane",
                    LastName = "Fellaini"
                },
                [RomeluLukakuId] = new FootballPlayer
                {
                    Id = RomeluLukakuId,
                    Age = 25,
                    CountryId = BelgiumId,
                    FirstName = "Romelu",
                    LastName = "Lukaku"
                },
            };
            _footballTeams = new Dictionary<string, FootballTeam>
            {
                {
                    ManchesterCityId,
                    new FootballTeam
                    {
                        Id = ManchesterCityId, Name = "Manchester City",
                        Players = new[] {_footballPlayers[VincentKompanyId],_footballPlayers[DeBruyneId]}
                    }
                },
                {
                    ManchesterUnitedId,
                    new FootballTeam
                    {
                        Id = ManchesterUnitedId, Name = "Manchester United",
                        Players = new[] {_footballPlayers[MarouaneFellainiId],_footballPlayers[RomeluLukakuId]}
                    }
                }
            };
        }

        public Country[] GetCountries()
        {
            return new[] {new Country {Id = EnglandId, Name = "England"},};
        }

        public FootballLeague[] GetLeagues(string countryId)
        {
            return new[]
            {
                new FootballLeague {Id = PremierLeagueId, Name = "Premier League", Level = 0, CountryId = EnglandId,CurrentSeasonId = PremierLeague2019Id},
            };
        }

        public FootballLeague GetLeague(string leagueId)
        {
            return new FootballLeague
            {
                Id = PremierLeagueId, Name = "Premier League", Level = 0, CountryId = EnglandId,
                CurrentSeasonId = PremierLeague2019Id
            };
        }

        public FootballLeagueSeason[] GetLeagueSeasons(string leagueId)
        {
            return new[]
            {
                new FootballLeagueSeason
                {
                    Id = PremierLeague2019Id, Name = "Premier League 2018/2019",
                    FootballLeagueId = PremierLeagueId,
                    Number = 0,
                    FootballClubIds = new []{ManchesterCityId,ManchesterUnitedId}
                },
            };
        }

        public FootballTeam[] GetSeasonTeams(string seasonId)
        {
            return new[] {ManchesterCityId, ManchesterUnitedId}.Select(x => _footballTeams[x]).ToArray();
        }

        public FootballPlayer[] GetPlayers() => _footballPlayers.Values.ToArray();

        public FootballPlayer[] GetTeamPlayers(string teamId)
        {
            return _footballTeams[teamId].Players;
        }
    }
}