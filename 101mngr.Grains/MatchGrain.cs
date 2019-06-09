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
        private IAsyncStream<MatchEventDto> _stream;
        private Guid _streamId;
        private IDisposable _matchTimer;

        private readonly MatchClock _matchClock = new MatchClock();

        public override Task OnActivateAsync()
        {
            _streamId = Guid.Parse(MatchId);
            var streamProvider = GetStreamProvider("SMSProvider");

            _stream = streamProvider.GetStream<MatchEventDto>(_streamId, "Matches");
            State = new MatchState {Id = MatchId, Players = new List<PlayerDataDto>()};
            return base.OnActivateAsync();
        }

        public Task<MatchInfoDto> GetMatchInfo()
        {
            return Task.FromResult(new MatchInfoDto
            {
                Id = MatchId, Name = State.Name, CreatedAt = DateTime.UtcNow, Players = State.Players.ToArray()
            });
        }

        public Task<MatchStateDto> GetMatchState()
        {
            return Task.FromResult(new MatchStateDto
            {
                Id = State.Id,
                Name = State.Name,
                StartTime = State.StartTime,
                Minute = State.Minute,
                MatchPeriod = State.MatchPeriod,
                Goals = State.Goals.ToDto(),
                YellowCards = State.YellowCards.ToDto(),
                RedCards = State.RedCards.ToDto(),
                HomeTeam = State.HomeTeam.ToDto(),
                AwayTeam = State.AwayTeam.ToDto(),
                MatchEvents = State.MatchEvents.Select(x => x.ToDto()).ToList()
            });
        }

        public async Task Start(TeamDto homeTeam, TeamDto awayTeam)
        {
            var matchRegistryGrain = GrainFactory.GetGrain<IMatchListGrain>(0);
            State.HomeTeam = new Team
            {
                Id = homeTeam.Id,
                Name = homeTeam.Name,
                Players = homeTeam.Players.Select(x => new MatchPlayer
                {
                    Id = x.Id,
                    Name = x.Name,
                    Bench = x.Bench,
                    PlayerType = x.PlayerType
                }).ToList()
            };
            State.AwayTeam = new Team
            {
                Id = awayTeam.Id,
                Name = awayTeam.Name,
                Players = awayTeam.Players.Select(x => new MatchPlayer
                {
                    Id = x.Id,
                    Name = x.Name,
                    Bench = x.Bench,
                    PlayerType = x.PlayerType
                }).ToList()
            };
            await matchRegistryGrain.AddMatch(MatchId, homeTeam, awayTeam);
            _matchTimer = RegisterTimer(MatchTimerImpl, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        public async Task FinishMatch()
        {
            foreach (var player in State.Players)
            {
                if (!player.IsVirtual)
                {
                    await PlayerHistory(player);
                }
            }

            var matchRegistryGrain = GrainFactory.GetGrain<IMatchListGrain>(0);
            await matchRegistryGrain.Remove(MatchId);
        }

        private Task HandleMatchEvent(MatchEventDto matchEventDto)
        {
            switch (matchEventDto.MatchEventType)
            {
                case MatchEventType.None:
                    break;
                case MatchEventType.Goal:
                    State.Apply(matchEventDto.ToGoalEvent());
                    break;
                case MatchEventType.YellowCard:
                    State.Apply(matchEventDto.ToYellowCardEvent());
                    break;
                case MatchEventType.RedCard:
                    State.Apply(matchEventDto.ToRedCardEvent());
                    break;
                case MatchEventType.Substitution:
                    State.Apply(matchEventDto.ToSubstitutionEvent());
                    break;
                case MatchEventType.Time:
                    State.Apply(matchEventDto.ToTimeEvent());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _stream.OnNextAsync(matchEventDto);
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

        private async Task MatchTimerImpl(object state)
        {
            _matchClock.Tick();

            var matchEventDto = new MatchEventDto
            {
                MatchPeriod = _matchClock.MatchPeriod,
                Minute = _matchClock.Minute,
                MatchEventType = MatchEventType.Time,
                MatchId = this.GetPrimaryKeyString(),
                Home = null,
            };
            await HandleMatchEvent(matchEventDto);

            if (_matchClock.MatchPeriod == MatchPeriod.FullTime)
            {
                _matchTimer?.Dispose();
                await FinishMatch();
            }
        }
    }

    // player joined
    // player leaved
    // players shuffled

    public struct Score
    {
        public Score(int home, int away)
        {
            Home = home;
            Away = away;
        }

        public int Home { get; }

        public int Away { get; }

        public Score HomeIncrement() => new Score(Home + 1, Away);
        public Score AwayIncrement() => new Score(Home, Away + 1);
    }

    public class Team
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IReadOnlyList<MatchPlayer> Players { get; set; }
    }

    public class MatchPlayer
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool? Home { get; set; }

        public bool Bench { get; set; }

        public PlayerType PlayerType { get; set; }

        public bool IsVirtual { get; set; } 
    }
}