using System;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;
using System.Collections.Generic;
using System.Linq;
using Orleans.Providers;

namespace _101mngr.Grains
{
    [StorageProvider(ProviderName = "OrleansStorage")]
    public class PlayerGrain : Grain<PlayerState>, IPlayerGrain
    {
        private long PlayerId => this.GetPrimaryKeyLong();
        
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
            await matchGrain.JoinMatch(PlayerId, State?.UserName);
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
