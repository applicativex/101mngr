using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;

namespace _101mngr.WebApp.Hubs
{
    public class MatchHub : Hub
    {
        private readonly MatchStream _matchStream;

        public MatchHub(MatchStream matchStream)
        {
            _matchStream = matchStream;
        }

        public Task<MatchStateDto> GetMatch(Guid matchStreamId) => _matchStream.GetMatch(matchStreamId);

        public Task<ChannelReader<MatchEventDto>> GetMatchStream(Guid matchStreamId)
        {
            return _matchStream.GetMatchStream(matchStreamId);
        }

        public Task<MatchDto[]> GetCurrentMatches()
        {
            return _matchStream.GetCurrentMatches();
        }

        public Task<ChannelReader<MatchDto>> GetCurrentMatchesStream()
        {
            return _matchStream.GetCurrentMatchesStream();
        }
    }

    public class MatchStream
    {
        private readonly IClusterClient _clusterClient;

        public MatchStream(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }
            
        public async Task<MatchDto[]> GetCurrentMatches()
        {
            var matchListGrain = _clusterClient.GetGrain<IMatchListGrain>(0);
            var matches = await matchListGrain.GetCurrentMatches();
            return matches;
        }   

        public Task<ChannelReader<MatchDto>> GetCurrentMatchesStream()
        {
            var streamProvider = _clusterClient.GetStreamProvider("SMSProvider");
            var matchStream = streamProvider.GetStream<MatchDto>(Guid.Empty, "MatchList");
            return matchStream.AsChannelReader();
        }

        public Task<ChannelReader<MatchEventDto>> GetMatchStream(Guid matchStreamId)
        {
            var streamProvider = _clusterClient.GetStreamProvider("SMSProvider");
            var matchStream = streamProvider.GetStream<MatchEventDto>(matchStreamId, "Matches");
            return matchStream.AsChannelReader();
        }

        public async Task<MatchStateDto> GetMatch(Guid matchStreamId)
        {
            var matchGrain = _clusterClient.GetGrain<IMatchGrain>(matchStreamId.ToString());
            var matchStateDto = await matchGrain.GetMatchState();
            return matchStateDto;
        }
    }
}