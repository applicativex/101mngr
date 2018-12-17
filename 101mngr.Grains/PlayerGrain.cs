using System;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;
using System.Collections.Generic;
using System.Linq;

namespace _101mngr.Grains
{
    public class PlayerGrain : Grain, IPlayerGrain
    {
        private long PlayerId => this.GetPrimaryKeyLong();

        protected PlayerState State;

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
            return Task.CompletedTask;
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
            return Task.CompletedTask;
        }

        public Task<MatchDto[]> GetMatchHistory()
        {
            return Task.FromResult(State.MatchHistory.OrderByDescending(x => x.CreatedAt).ToArray());
        }

        protected class PlayerState
        {
            public long Id { get; set; }

            public string UserName { get; set; }

            public string Email { get; set; }

            public string CountryCode { get; set; }

            public List<MatchDto> MatchHistory { get; set; }
        }
    }
}
