using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Orleans.Streams;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;

namespace _101mngr.Grains
{
    public class MatchListGrain : Grain, IMatchListGrain
    {
        private IAsyncStream<MatchDto> _matchListStream;
        private readonly Dictionary<Guid, StreamSubscriptionHandle<MatchEventDto>> _matchStreams = new Dictionary<Guid, StreamSubscriptionHandle<MatchEventDto>>();

        private readonly Dictionary<string, MatchListItem> _matches = new Dictionary<string, MatchListItem>();
        private readonly List<MatchListItem> _finishedMatches = new List<MatchListItem>();

        public override Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider("SMSProvider");
            _matchListStream = streamProvider.GetStream<MatchDto>(Guid.Empty, "MatchList");
            return base.OnActivateAsync();
        }

        public Task<MatchDto[]> GetCurrentMatches()
        {
            var result = _matches.Values
                .Select(x => ToMatchDto(x, MatchListEventType.None))
                .OrderByDescending(x => x.CreatedAt)
                .ToArray();
            return Task.FromResult(result);
        }

        public Task<MatchDto[]> GetFinishedMatches()
        {
            var result = _finishedMatches
                .Select(x => ToMatchDto(x, MatchListEventType.None))
                .OrderByDescending(x => x.CreatedAt)
                .ToArray();
            return Task.FromResult(result);
        }

        public async Task AddMatch(string matchId, TeamDto homeTeam, TeamDto awayTeam)
        {
            var matchStreamId = Guid.Parse(matchId);
            var streamProvider = GetStreamProvider("SMSProvider");
            var matchStream = streamProvider.GetStream<MatchEventDto>(matchStreamId, "Matches");
            var matchItem = _matches[matchId] = new MatchListItem(matchId, new Identifier
                {
                    Id = homeTeam.Id, Name = homeTeam.Name
                },
                new Identifier
                {
                    Id = awayTeam.Id, Name = awayTeam.Name
                });
            await _matchListStream.OnNextAsync(ToMatchDto(matchItem, MatchListEventType.MatchAdded));

            var subscriptionHandle = await matchStream.SubscribeAsync(OnMatchEvent);
            _matchStreams[matchStreamId] = subscriptionHandle;
        }

        public async Task Remove(string matchId)
        {
            if (_matches.TryGetValue(matchId, out var matchItem))
            {
                _matches.Remove(matchId);
                if (_matchStreams.TryGetValue(Guid.Parse(matchId), out var subscriptionHandle))
                {
                    await subscriptionHandle.UnsubscribeAsync();
                }
                _finishedMatches.Add(matchItem);
                await _matchListStream.OnNextAsync(ToMatchDto(matchItem, MatchListEventType.MatchRemoved));
            }
        }

        private async Task OnMatchEvent(MatchEventDto matchEventDto, StreamSequenceToken streamSequenceToken)
        {
            if (_matches.TryGetValue(matchEventDto.MatchId, out var matchItem))
            {
                var matchListEventType = MatchListEventType.None;

                if (matchEventDto.MatchEventType == MatchEventType.Goal)
                {
                    matchItem.Goal(matchEventDto.Home.GetValueOrDefault());
                    matchListEventType = MatchListEventType.MatchGoal;
                }

                if (matchEventDto.MatchEventType == MatchEventType.Time)
                {
                    matchItem.Time(matchEventDto.MatchPeriod, matchEventDto.Minute);
                    matchListEventType = MatchListEventType.MatchTime;
                }

                if (matchListEventType != MatchListEventType.None)
                {
                    await _matchListStream.OnNextAsync(ToMatchDto(matchItem, matchListEventType));
                }
            }
            else
            {
                Console.WriteLine("invalid match event");
            }
        }

        private static MatchDto ToMatchDto(MatchListItem matchItem, MatchListEventType eventType)
        {
            return new MatchDto
            {
                Id = matchItem.MatchId,
                Name = $"{matchItem.HomeTeam.Name} - {matchItem.AwayTeam.Name}",
                Minute = matchItem.Minute,
                MatchPeriod = matchItem.MatchPeriod,
                MatchListEventType = eventType,
                HomeTeam = new TeamDto
                {
                    Id = matchItem.HomeTeam.Id,
                    Name = matchItem.HomeTeam.Name,
                    Players = new List<MatchPlayerDto>()
                },
                AwayTeam = new TeamDto
                {
                    Id = matchItem.AwayTeam.Id,
                    Name = matchItem.AwayTeam.Name,
                    Players = new List<MatchPlayerDto>()
                },
                Goals = new ScoreDto {Home = matchItem.Goals.Home, Away = matchItem.Goals.Away}
            };
        }

        private class MatchListItem
        {
            public MatchListItem(string matchId, Identifier homeTeam, Identifier awayTeam)
            {
                MatchId = matchId;
                HomeTeam = homeTeam;
                AwayTeam = awayTeam;
                CreatedAt = DateTime.UtcNow;
            }

            public string MatchId { get; }

            public Identifier HomeTeam { get; }

            public Identifier AwayTeam { get; }

            public DateTime CreatedAt { get; }
            
            public MatchPeriod MatchPeriod { get; private set; }

            public int Minute { get; private set; }  

            public Score Goals { get; private set; }

            public void Goal(bool home)
            {
                Goals = home ? Goals.HomeIncrement() : Goals.AwayIncrement();
            }

            public void Time(MatchPeriod matchPeriod, int minute)
            {
                Minute = minute;
                MatchPeriod = matchPeriod;
            }
        }
    }
}