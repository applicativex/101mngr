using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;
using _101mngr.Leagues;

namespace _101mngr.Grains   
{
    public class MatchRoomGrain : Grain, IMatchRoomGrain
    {
        private static string[] _randomTeamNames = new[]
            {"Tigers", "Roasters", "Goose United", "Tagger 5", "Zombieland", "Trouble FC", "Jacob Priests", "Veggi Boom FC", "1$lil_s0x", "Old Fashioned Bastards"};
        private readonly LeagueService _leagueService;
        private MatchPlayer[] _virtualPlayers = new MatchPlayer[22];
        private readonly List<MatchPlayer> _players = new List<MatchPlayer>();
        private string _ownerId;

        private Func<int, bool> _randomizer = x => x % 2 == 0;
            
        public MatchRoomGrain(LeagueService leagueService)
        {
            _leagueService = leagueService;
        }

        public override Task OnActivateAsync()
        {
            _virtualPlayers = _leagueService.GetPlayers()
                .Take(22)
                .Select(x => new MatchPlayer {Id = x.Id, Name = Name(x.FirstName, x.LastName)})
                .ToArray();

            return base.OnActivateAsync();
        }

        public Task<MatchRoomDto> GetMatchRoom()
        {
            var result = new MatchRoomDto
            {
                MatchId = this.GetPrimaryKeyString(),
                OwnerPlayerId =  _ownerId,
                Players = _players.Select(Convert).ToArray(),
                VirtualPlayers = _virtualPlayers.Select(Convert).ToArray(),
            };  
            return Task.FromResult(result);
        }

        public async Task NewRoom(long playerId, string playerName)
        {
            _ownerId = playerId.ToString();
            _players.Add(new MatchPlayer
            {
                Id = playerId.ToString(),
                Name = playerName
            });
            var matchRoomRegistryGrain = GrainFactory.GetGrain<IMatchRoomRegistryGrain>(0);
            await matchRoomRegistryGrain.AddMatchRoom(playerId, this.GetPrimaryKeyString());
        }

        public Task<bool> Join(long playerId, string playerName)
        {
            var result = false;
            if (!_players.Exists(x => x.Id == playerId.ToString()))
            {
                if (_players.Count < 22)
                {
                    _players.Add(new MatchPlayer {Id = playerId.ToString(), Name = playerName});
                    result = true;
                }
            }

            return Task.FromResult(result);
        }

        public Task<bool> Leave(long playerId)
        {
            var result = false;
            var player = _players.SingleOrDefault(x => x.Id == playerId.ToString());
            if (player != null)
            {
                _players.Remove(player);
                result = true;
            }

            return Task.FromResult(result);
        }

        public async Task StartMatch()
        {
            var random = new Random();
            var homeTeamName = _randomTeamNames[random.Next(0, _randomTeamNames.Length - 1)];
            var awayTeamName = _randomTeamNames[random.Next(0, _randomTeamNames.Length - 1)];
            await RandomTeams();
            var matchGrain = GrainFactory.GetGrain<IMatchGrain>(this.GetPrimaryKeyString());
            var zipPlayers = _virtualPlayers.Zip(_players, (x, y) => y ?? x).ToArray();
            var homeTeam = new TeamDto
            {
                Id = Guid.NewGuid().ToString(), 
                Name = homeTeamName,
                Players = zipPlayers.Where(x => x.Home.Value).Select(x => new MatchPlayerDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToArray()
            };
            var awayTeam = new TeamDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = awayTeamName,
                Players = zipPlayers.Where(x => !x.Home.Value).Select(x => new MatchPlayerDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToArray()
            };
            await matchGrain.Start(homeTeam, awayTeam);
            var matchRoomRegistryGrain = GrainFactory.GetGrain<IMatchRoomRegistryGrain>(0);
            await matchRoomRegistryGrain.RemoveMatchRoom(this.GetPrimaryKeyString());
        }

        private Task RandomTeams()
        {
            for (var i = 0; i < 22; i++)
            {
                var player = GetPlayerByIndex(i);

                if (_randomizer(i))
                {
                    if (player != null)
                    {
                        player.Home = true;
                    }

                    _virtualPlayers[i].Home = true;
                }
                else
                {
                    if (player != null)
                    {
                        player.Home = false;
                    }

                    _virtualPlayers[i].Home = false;
                }
            }

            _randomizer = x => !_randomizer(x);

            return Task.CompletedTask;

            MatchPlayer GetPlayerByIndex(int index)
            {
                if (_players.Count > index)
                {
                    return _players[index];
                }

                return null;
            }
        }

        private static string Name(string firstName, string lastName) => $"{firstName} {lastName}".Trim();

        private static MatchPlayerDto Convert(MatchPlayer value) =>
            value != null ? new MatchPlayerDto { Id = value.Id, Name = value.Name, } : null;
    }
}   