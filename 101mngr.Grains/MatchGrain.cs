using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;

namespace _101mngr.Grains
{
    public class MatchGrain : Grain, IMatchGrain
    {
        private string MatchId => this.GetPrimaryKeyString();
        private MatchState State;

        public override Task OnActivateAsync()
        {
            State = new MatchState {Id = MatchId};
            return base.OnActivateAsync();
        }
        
        public async Task NewMatch(PlayerDataDto player)
        {
            State.Players.Add(new PlayerDataDto
            {
                Id = player.Id,
                Level = player.Level,
                PlayerType = player.PlayerType,
                UserName = player.UserName
            });
            var matchRegistryGrain = GrainFactory.GetGrain<IMatchRegistryGrain>(0);
            await matchRegistryGrain.Register(new MatchDto {Id = MatchId});
        }

        public Task JoinMatch(PlayerDataDto player)
        {
            State.Players.Add(new PlayerDataDto
            {
                Id = player.Id,
                Level = player.Level,
                PlayerType = player.PlayerType,
                UserName = player.UserName
            });
            return Task.CompletedTask;
        }

        public Task LeaveMatch(long playerId)
        {
            State.Players.RemoveAll(x => x.Id == playerId);
            return Task.CompletedTask;
        }

        public Task PickCaptains()
        {
            var (team1, team2) = GetRandomCaptainIds();

            State.Players[team1].IsCaptain = true;
            State.Players[team2].IsCaptain = true;

            return Task.CompletedTask;

            (int, int) GetRandomCaptainIds()
            {
                var random = new Random();
                int captainTeam2Index;
                var captainTeam1Index = random.Next(State.Players.Count - 1);
                do
                {
                    captainTeam2Index = random.Next(State.Players.Count - 1);
                } while (captainTeam1Index != captainTeam2Index);

                return (captainTeam1Index, captainTeam2Index);
            }
        }

        public Task<PlayerDataDto[]> GetPlayers()
        {
            return Task.FromResult(State.Players.ToArray());
        }

        public Task StartMatch()
        {
            throw new NotImplementedException();
        }

        public Task PickPlayer(int team, long playerId)
        {
            throw new NotImplementedException();
        }

        private class MatchState
        {
            public string Id { get; set; }

            public List<PlayerDataDto> Players { get; set; }

        }
    }
}