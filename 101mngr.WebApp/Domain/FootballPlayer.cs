using System;
using System.Collections.Generic;

namespace _101mngr.WebApp.Domain
{
    public enum FootballPlayerType
    {
        Goalkeeper = 1,
        Defender,
        Midfielder,
        Forward
    }

    public class FootballPlayer
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public int Level { get; set; }

        public string CountryId { get; set; }
    }

    public class FootballClub
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public FootballPlayer[] Players { get; set; }
    }

    /// <summary>
    /// Static description of soccer league e.g. Premier League, Primera, Bundesliga
    /// </summary>
    public class FootballLeague
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryId { get; set; }

        public int Level { get; set; }
    }

    public class FootballLeagueSeason
    {
        public string Id { get; set; }

        public string FootballLeagueId { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public string[] FootballClubIds { get; set; }
    }

    public class FootballMatch
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string HomeTeamId { get; set; }

        public string AwayTeamId { get; set; }

        public DateTime StartDate { get; set; }

        public string FootballLeagueSeasonId { get; set; }
    }

    public class Country
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class FootballMatchService
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

        private readonly Dictionary<string, FootballClub> _footballClubs;

        public FootballMatchService()
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
            _footballClubs = new Dictionary<string, FootballClub>
            {
                {
                    ManchesterCityId,
                    new FootballClub
                    {
                        Id = ManchesterCityId, Name = "Manchester City",
                        Players = new[] {_footballPlayers[VincentKompanyId],_footballPlayers[DeBruyneId]}
                    }
                },
                {
                    ManchesterUnitedId,
                    new FootballClub
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
                new FootballLeague {Id = PremierLeagueId, Name = "Premier League", Level = 0, CountryId = EnglandId},
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
    }
}