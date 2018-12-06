using System;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Enums;
using _101mngr.Contracts.Models;

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
                CountryCode = request.CountryCode
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

        protected class PlayerState
        {
            public long Id { get; set; }

            public string UserName { get; set; }

            public string Email { get; set; }

            public string CountryCode { get; set; } 
        }
    }
}
