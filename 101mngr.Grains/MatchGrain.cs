using System;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;
using System.Linq;
using Orleans.Concurrency;
using Orleans.Streams;
using _101mngr.Domain;
using _101mngr.Domain.Enums;
using Team = _101mngr.Domain.Team;

namespace _101mngr.Grains
{
    [Reentrant]
    public class MatchGrain : Grain, IMatchGrain
    {
        private string MatchId => this.GetPrimaryKeyString();
        private Match State;
        private IAsyncStream<MatchEventDto> _stream;
        private Guid _streamId;
        private IDisposable _matchTimer;
        private bool _halfTime;

        private readonly MatchClock _matchClock = new MatchClock();

        public override Task OnActivateAsync()
        {
            _streamId = Guid.Parse(MatchId);
            var streamProvider = GetStreamProvider("SMSProvider");

            _stream = streamProvider.GetStream<MatchEventDto>(_streamId, "Matches");
            State = new Match {Id = MatchId};
            return base.OnActivateAsync();
        }

        public Task<MatchDto> GetMatchInfo()
        {
            return Task.FromResult(State.ToDto());
        }

        public async Task Start(TeamDto homeTeam, TeamDto awayTeam)
        {
            var matchRegistryGrain = GrainFactory.GetGrain<IMatchRegistryGrain>(0);
            State.HomeTeam = new Team
            {
                Id = homeTeam.Id,
                Name = homeTeam.Name,
                Players = homeTeam.Players.Select(x => new MatchPlayer
                {
                    Id = x.Id,
                    Name = x.Name,
                    Bench = x.Bench,
                    PlayerType = (PlayerType) x.PlayerType
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
                    PlayerType = (PlayerType) x.PlayerType
                }).ToList()
            };
            await matchRegistryGrain.AddMatch(MatchId, homeTeam, awayTeam);
            _matchTimer = RegisterTimer(MatchTimerImpl, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        public async Task FinishMatch()
        {
            var matchHistoryGrain = GrainFactory.GetGrain<IMatchHistoryGrain>(0);
            await matchHistoryGrain.AddMatchHistory(State.ToDto());

            var matchRegistryGrain = GrainFactory.GetGrain<IMatchRegistryGrain>(0);
            await matchRegistryGrain.RemoveMatch(MatchId);
        }

        private Task HandleMatchEvent(MatchEventDto matchEventDto)
        {
            switch ((MatchEventType) matchEventDto.MatchEventType)
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

        private async Task MatchTimerImpl(object state)
        {
            _matchClock.Tick();

            if (await HalfTime())
            {
                return;
            }

            _halfTime = _matchClock.MatchPeriod == MatchPeriod.HalfTime;

            if (!_halfTime)
            {
                var matchEventDto = RandomGoalEvent(_matchClock.MatchPeriod, _matchClock.Minute)
                                    ?? RandomYellowCard(_matchClock.MatchPeriod, _matchClock.Minute)
                                    ?? RandomRedCard(_matchClock.MatchPeriod, _matchClock.Minute)
                                    ?? TimeEvent();
                await HandleMatchEvent(matchEventDto);

                if (_matchClock.MatchPeriod == MatchPeriod.FullTime)
                {
                    _matchTimer?.Dispose();
                    await FinishMatch();
                }
            }

            async Task<bool> HalfTime()
            {
                if (!_halfTime && _matchClock.MatchPeriod == MatchPeriod.HalfTime)
                {
                    _halfTime = true;
                    await HandleMatchEvent(new MatchEventDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        MatchPeriod = (int) _matchClock.MatchPeriod,
                        Minute = _matchClock.Minute,
                        MatchEventType = (int) MatchEventType.Time,
                        MatchId = this.GetPrimaryKeyString(),
                        Home = null,
                    });
                    return true;
                }

                return false;
            }
        }

        private MatchEventDto TimeEvent()
        {
            return new MatchEventDto
            {
                Id = Guid.NewGuid().ToString(),
                MatchPeriod = (int) _matchClock.MatchPeriod,
                Minute = _matchClock.Minute,
                MatchEventType = (int) MatchEventType.Time,
                MatchId = this.GetPrimaryKeyString(),
                Home = null,
            };
        }

        private MatchEventDto RandomGoalEvent(MatchPeriod matchPeriod, int minute)
        {
            var rnd = new Random();
            if (rnd.Next(1, 100) % 13 == 0)
            {
                var home = rnd.Next(100) % 2 == 0;
                var playerIndex = rnd.Next(0, State.HomeTeam.Players.Count - 1);
                var player = (home ? State.HomeTeam : State.AwayTeam).Players[playerIndex];
                return new MatchEventDto
                {
                    Id = Guid.NewGuid().ToString(),
                    Home = home,
                    MatchPeriod = (int) matchPeriod,
                    MatchId = State.Id,
                    Minute = minute,
                    MatchEventType = (int) MatchEventType.Goal,
                    PlayerId = player.Id
                };
            }

            return null;
        }

        private MatchEventDto RandomYellowCard(MatchPeriod matchPeriod, int minute)
        {
            var rnd = new Random();
            if (rnd.Next(1, 100) % 19 == 0)
            {
                var home = rnd.Next(100) % 2 == 0;
                var playerIndex = rnd.Next(0, State.HomeTeam.Players.Count - 1);
                var player = (home ? State.HomeTeam : State.AwayTeam).Players[playerIndex];
                return new MatchEventDto
                {
                    Id = Guid.NewGuid().ToString(),
                    Home = home,
                    MatchPeriod = (int) matchPeriod,
                    MatchId = State.Id,
                    Minute = minute,
                    MatchEventType = (int) MatchEventType.YellowCard,
                    PlayerId = player.Id
                };
            }

            return null;
        }

        private MatchEventDto RandomRedCard(MatchPeriod matchPeriod, int minute)
        {
            var rnd = new Random();
            if (rnd.Next(1, 100) % 37 == 0)
            {
                var home = rnd.Next(100) % 2 == 0;
                var playerIndex = rnd.Next(0, State.HomeTeam.Players.Count - 1);
                var player = (home ? State.HomeTeam : State.AwayTeam).Players[playerIndex];
                return new MatchEventDto
                {
                    Id = Guid.NewGuid().ToString(),
                    Home = home,
                    MatchPeriod = (int) matchPeriod,
                    MatchId = State.Id,
                    Minute = minute,
                    MatchEventType = (int) MatchEventType.RedCard,
                    PlayerId = player.Id
                };
            }

            return null;
        }
    }
}