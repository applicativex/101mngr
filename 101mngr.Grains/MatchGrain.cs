using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Enums;
using _101mngr.Contracts.Models;

namespace _101mngr.Grains
{
    public class MatchGrain : Grain, IMatchGrain
    {
        private string MatchId => this.GetPrimaryKeyString();
        private MatchState State;

        public override Task OnActivateAsync()
        {
            State = new MatchState {Id = MatchId,Players = new List<PlayerDataDto>()};
            return base.OnActivateAsync();
        }

        public Task<MatchInfoDto> GetMatchInfo()
        {
            return Task.FromResult(new MatchInfoDto
            {
                Id = MatchId, Name = State.Name, CreatedAt = DateTime.UtcNow, Players = State.Players.ToArray()
            });
        }

        public async Task NewMatch(long playerId, string playerName, string matchName)
        {
            State.Players.Add(new PlayerDataDto
            {
                Id = playerId,
                Level = 10,
                PlayerType = PlayerType.Midfielder,
                UserName = playerName
            });
            State.Name = matchName;
            var matchRegistryGrain = GrainFactory.GetGrain<IMatchListGrain>(0);
            await matchRegistryGrain.Add(MatchId, State.Name);
        }

        public Task JoinMatch(long playerId, string playerName)
        {
            State.Players.Add(new PlayerDataDto
            {
                Id = playerId,
                Level = 10,
                PlayerType = PlayerType.Midfielder,
                UserName = playerName
            });
            return Task.CompletedTask;
        }

        public Task LeaveMatch(long playerId)
        {
            State.Players.RemoveAll(x => x.Id == playerId);
            return Task.CompletedTask;
        }
        
        public async Task PlayMatch()
        {
            var matchRegistryGrain = GrainFactory.GetGrain<IMatchListGrain>(0);
            await matchRegistryGrain.Remove(MatchId);
        }

        private class MatchState
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public DateTime CreatedAt { get; set; } 

            public List<PlayerDataDto> Players { get; set; }

        }
    }
}