using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Enums;
using _101mngr.Contracts.Models;
using System.Linq;
using Orleans.Concurrency;
using Orleans.Streams;

namespace _101mngr.Grains
{
    [Reentrant]
    public class MatchGrain : Grain, IMatchGrain
    {
        private string MatchId => this.GetPrimaryKeyString();
        private MatchState State;
        private IAsyncStream<MatchListItemDto> _stream;
        private Guid _streamId;

        public override Task OnActivateAsync()
        {
            _streamId = Guid.Parse(MatchId);
            Console.WriteLine($"Activated as {_streamId}");
            var streamProvider = GetStreamProvider("SMSProvider");
            
            _stream = streamProvider.GetStream<MatchListItemDto>(_streamId, "Matches");
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

        private async Task PublishMatchStateUpdate()
        {
            var streamProvider = GetStreamProvider("SMSProvider");
            var stream = streamProvider.GetStream<MatchStateData>(Guid.Empty, "matchStateStream");

            await stream.OnNextAsync(new MatchStateData { });
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
            await _stream.OnNextAsync(new MatchListItemDto
            {
                Id = MatchId,
                Name = State.Name,
                PlayersCount = State.Players.Count
            });
        }

        // todo: add player types for virtual players and real
        public async Task JoinMatch(long playerId, string playerName, bool isVirtualPlayer)
        {
            State.Players.Add(new PlayerDataDto
            {
                Id = playerId,
                Level = 10, // todo: consolidate player level
                PlayerType = PlayerType.Midfielder, // todo: consolidate player type
                UserName = playerName
            });
            await _stream.OnNextAsync(new MatchListItemDto
            {
                Id = MatchId,
                Name = State.Name,
                PlayersCount = State.Players.Count
            });
        }

        public Task LeaveMatch(long playerId)
        {
            State.Players.RemoveAll(x => x.Id == playerId);
            return Task.CompletedTask;
        }
        
        public async Task PlayMatch()
        {
            foreach (var player in State.Players)
            {
                await PlayerHistory(player);
            }
            var matchRegistryGrain = GrainFactory.GetGrain<IMatchListGrain>(0);
            await matchRegistryGrain.Remove(MatchId);
        }

        private async Task PlayerHistory(PlayerDataDto dto)
        {
            if (dto.IsVirtual)
            {
                return;
            }

            var playerGrain = GrainFactory.GetGrain<IPlayerGrain>(dto.Id.Value);
            await playerGrain.AddMatchHistory(new MatchDto()
            {
                Id = MatchId,
                Name = State.Name,
                CreatedAt = State.CreatedAt
            });
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