using System.Collections.Generic;

namespace _101mngr.Leagues
{
    public class LeagueDbContext
    {
        private const string EnglandId = "1";
        private const string FranceId = "2";
        private const string BelgiumId = "3";
        private const string ChileId = "4";
        private const string BrazilId = "5";
        private const string SpainId = "6";
        private const string ArgentineId = "7";
        private const string UkraineId = "8";
        private const string EquadorId = "9";

        private const string PremierLeagueId = "1";
        private const string PremierLeague2019Id = "1";
        private const string ManchesterCityId = "1";
        private const string ManchesterUnitedId = "2";

        private const string DeBruyneId = "1000";
        private const string VincentKompanyId = "2000";
        private const string ClaudioBravoId = "5000";
        private const string KyleWalkerId = "6000";
        private const string LukeBoltonId = "7000";
        private const string FernandinhoId = "8000";
        private const string DavidSilvaId = "9000";
        private const string SergioAgueroId = "1000";
        private const string OlexandrZinchenkoId = "1100";
        private const string AymericLaporteId = "1200";
        private const string SterlingRaheemId = "1300";

        private const string MarouaneFellainiId = "3000";
        private const string RomeluLukakuId = "4000";
        private const string SergioRomeroId = "1400";
        private const string AntonioValenciaId = "1500";
        private const string MarcusRashfordId = "1600";
        private const string AlexisSanchezId = "1700";
        private const string JesseLingardId = "1800";
        private const string AnderHerreraId = "1900";
        private const string FredId = "2000";
        private const string PaulPogbaId = "2100";
        private const string AshleyYoungId = "2200";

        public LeagueDbContext()
        {
            Players = new Dictionary<string, FootballPlayer>
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
                [ClaudioBravoId] = new FootballPlayer
                {
                    Id = ClaudioBravoId,
                    Age = 35,
                    CountryId = ChileId,
                    FirstName = "Claudio",
                    LastName = "Bravo"
                },
                [KyleWalkerId] = new FootballPlayer
                {
                    Id = KyleWalkerId,
                    Age = 28,
                    CountryId = EnglandId,
                    FirstName = "Kyle",
                    LastName = "Walker"
                },
                [LukeBoltonId] = new FootballPlayer
                {
                    Id = LukeBoltonId,
                    Age = 19,
                    CountryId = EnglandId,
                    FirstName = "Luke",
                    LastName = "Bolton"
                },
                [FernandinhoId] = new FootballPlayer
                {
                    Id = FernandinhoId,
                    Age = 33,
                    CountryId = BrazilId,
                    FirstName = "Fernandinho",
                    LastName = ""
                },
                [DavidSilvaId] = new FootballPlayer
                {
                    Id = DavidSilvaId,
                    Age = 33,
                    CountryId = SpainId,
                    FirstName = "David",
                    LastName = "Silva"
                },
                [SergioAgueroId] = new FootballPlayer
                {
                    Id = SergioAgueroId,
                    Age = 30,
                    CountryId = ArgentineId,
                    FirstName = "Sergio",
                    LastName = "Aguero"
                },
                [OlexandrZinchenkoId] = new FootballPlayer
                {
                    Id = OlexandrZinchenkoId,
                    Age = 22,
                    CountryId = UkraineId,
                    FirstName = "Olexandr",
                    LastName = "Zinchenko"
                },
                [AymericLaporteId] = new FootballPlayer
                {
                    Id = AymericLaporteId,
                    Age = 24,
                    CountryId = FranceId,
                    FirstName = "Aymeric",
                    LastName = "Laporte"
                },
                [SterlingRaheemId] = new FootballPlayer
                {
                    Id = SterlingRaheemId,
                    Age = 24,
                    CountryId = EnglandId,
                    FirstName = "Raheem",
                    LastName = "Sterling"
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
                [SergioRomeroId] = new FootballPlayer
                {
                    Id = SergioRomeroId,
                    Age = 21,
                    CountryId = ArgentineId,
                    FirstName = "Sergio",
                    LastName = "Romero"
                },
                [AntonioValenciaId] = new FootballPlayer
                {
                    Id = AntonioValenciaId,
                    Age = 33,
                    CountryId = EquadorId,
                    FirstName = "Antonio",
                    LastName = "Valencia"
                },
                [MarcusRashfordId] = new FootballPlayer
                {
                    Id = MarcusRashfordId,
                    Age = 21,
                    CountryId = EnglandId,
                    FirstName = "Marcus",
                    LastName = "Rashford"
                },
                [AlexisSanchezId] = new FootballPlayer
                {
                    Id = AlexisSanchezId,
                    Age = 30,
                    CountryId = ChileId,
                    FirstName = "Alexis",
                    LastName = "Sanchez"
                },
                [JesseLingardId] = new FootballPlayer
                {
                    Id = JesseLingardId,
                    Age = 26,
                    CountryId = EnglandId,
                    FirstName = "Jesse",
                    LastName = "Lingard"
                },
                [AnderHerreraId] = new FootballPlayer
                {
                    Id = AnderHerreraId,
                    Age = 29,
                    CountryId = SpainId,
                    FirstName = "Ander",
                    LastName = "Herrera"
                },
                [FredId] = new FootballPlayer
                {
                    Id = FredId,
                    Age = 25,
                    CountryId = BrazilId,
                    FirstName = "Fred",
                    LastName = ""
                },
                [PaulPogbaId] = new FootballPlayer
                {
                    Id = PaulPogbaId,
                    Age = 25,
                    CountryId = FranceId,
                    FirstName = "Paul",
                    LastName = "Pogba"
                },
                [AshleyYoungId] = new FootballPlayer
                {
                    Id = AshleyYoungId,
                    Age = 33,
                    CountryId = EnglandId,
                    FirstName = "Ashley",
                    LastName = "Young"
                },
            };

            Teams = new Dictionary<string, FootballTeam>
            {
                {
                    ManchesterCityId,
                    new FootballTeam
                    {
                        Id = ManchesterCityId, Name = "Manchester City",
                        Players = new[]
                        {
                            Players[VincentKompanyId], Players[DeBruyneId], Players[OlexandrZinchenkoId],
                            Players[ClaudioBravoId], Players[KyleWalkerId], Players[FernandinhoId],
                            Players[LukeBoltonId],
                            Players[DavidSilvaId], Players[SergioAgueroId], Players[AymericLaporteId],
                            Players[SterlingRaheemId]
                        }
                    }
                },
                {
                    ManchesterUnitedId,
                    new FootballTeam
                    {
                        Id = ManchesterUnitedId, Name = "Manchester United",
                        Players = new[]
                        {
                            Players[MarouaneFellainiId], Players[RomeluLukakuId], Players[AshleyYoungId],
                            Players[FredId], Players[PaulPogbaId], Players[JesseLingardId], Players[AnderHerreraId],
                            Players[AlexisSanchezId], Players[MarcusRashfordId], Players[SergioRomeroId],
                            Players[AntonioValenciaId]
                        }
                    }
                }
            };

            Leagues = new Dictionary<string, FootballLeague>
            {
                {
                    PremierLeagueId,
                    new FootballLeague
                    {
                        Id = PremierLeagueId, Name = "Premier League", Level = 0, CountryId = EnglandId,
                        CurrentSeasonId = PremierLeague2019Id
                    }
                }
            };

            Seasons = new Dictionary<string, FootballLeagueSeason>
            {
                {
                    PremierLeague2019Id, new FootballLeagueSeason
                    {
                        Id = PremierLeague2019Id, Name = "Premier League 2018/2019",
                        LeagueId = PremierLeagueId,
                        Number = 0,
                        TeamIds = new[] {ManchesterCityId, ManchesterUnitedId}
                    }
                }
            };

            Countries = new[] {new Country {Id = EnglandId, Name = "England"}};
        }

        public static LeagueDbContext Default = new LeagueDbContext();

        public Country[] Countries { get; set; }
        public Dictionary<string, FootballLeague> Leagues { get; set; }
        public Dictionary<string, FootballLeagueSeason> Seasons { get; set; }
        public Dictionary<string, FootballPlayer> Players { get; set; }
        public Dictionary<string, FootballTeam> Teams { get; set; }
    }
}