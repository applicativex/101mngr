using System;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;
using System.Collections.Generic;
using System.Linq;
using Orleans.Providers;
using _101mngr.Leagues;

namespace _101mngr.Grains
{
    [StorageProvider(ProviderName = "OrleansStorage")]
    public class PlayerGrain : Grain<PlayerState>, IPlayerGrain
    {
        private readonly LeagueService _leagueService;
        private long PlayerId => this.GetPrimaryKeyLong();

        public PlayerGrain(LeagueService leagueService)
        {
            _leagueService = leagueService;
        }
        
        public Task<long> GetPlayer()
        {
            return Task.FromResult(PlayerId);
        }

        public Task Create(CreatePlayerDto request)
        {
            State = new PlayerState
            {
                Id = PlayerId,
                UserName = request.UserName,
                Email = request.Email,
                CountryCode = request.CountryCode,
                MatchHistory = new List<MatchDto>()
            };
            return WriteStateAsync();
        }

        public Task<PlayerDto> GetPlayerInfo()
        {
            throw new NotImplementedException();
        }

        public async Task<string> NewMatch(string matchName)
        {
            var matchId = $"{PlayerId}:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            var matchGrain = GrainFactory.GetGrain<IMatchGrain>(matchId);
            await matchGrain.NewMatch(PlayerId, State?.UserName, matchName);
            return matchId;
        }

        public async Task JoinMatch(string matchId)
        {
            var matchGrain = GrainFactory.GetGrain<IMatchGrain>(matchId);
            await matchGrain.JoinMatch(PlayerId, State?.UserName, false);
        }

        public async Task LeaveMatch(string matchId)
        {
            var matchGrain = GrainFactory.GetGrain<IMatchGrain>(matchId);
            await matchGrain.LeaveMatch(PlayerId);
        }

        public Task AddMatchHistory(MatchDto match)
        {
            State.MatchHistory.Add(match);
            return WriteStateAsync();
        }

        public async Task<string> RandomMatch()
        {
            var matchId = $"{PlayerId}:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            var matchGrain = GrainFactory.GetGrain<IMatchGrain>(matchId);
            await matchGrain.NewMatch(PlayerId, State.UserName,
                $"Random Match {DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
            var virtualPlayers = _leagueService.GetPlayers().Take(21).ToArray();
            foreach (var virtualPlayer in virtualPlayers)
            {
                // todo: handle id long vs string
                await matchGrain.JoinMatch(
                    long.Parse(virtualPlayer.Id), $"{virtualPlayer.FirstName} {virtualPlayer.LastName}", true);
            }

            await matchGrain.PlayMatch();
            return matchId;
        }

        public Task<MatchDto[]> GetMatchHistory()
        {
            return Task.FromResult(State.MatchHistory.OrderByDescending(x => x.CreatedAt).ToArray());
        }
    }

    public class PlayerState
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string CountryCode { get; set; }

        public List<MatchDto> MatchHistory { get; set; }
    }
}
