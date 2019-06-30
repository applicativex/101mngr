using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;

namespace _101mngr.WebApp.Hubs
{
    public class MatchService
    {
        private readonly IClusterClient _clusterClient;

        public MatchService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }
            
        public async Task<MatchListItemDto[]> GetCurrentMatches()
        {
            var matchListGrain = _clusterClient.GetGrain<IMatchRegistryGrain>(0);
            var matches = await matchListGrain.GetCurrentMatches();
            return matches;
        }   

        public Task<ChannelReader<MatchListItemDto>> GetCurrentMatchesStream()
        {
            var streamProvider = _clusterClient.GetStreamProvider("SMSProvider");
            var matchStream = streamProvider.GetStream<MatchListItemDto>(Guid.Empty, "MatchList");
            return matchStream.AsChannelReader();
        }

        public Task<ChannelReader<MatchEventDto>> GetMatchStream(Guid matchStreamId)
        {
            var streamProvider = _clusterClient.GetStreamProvider("SMSProvider");
            var matchStream = streamProvider.GetStream<MatchEventDto>(matchStreamId, "Matches");
            return matchStream.AsChannelReader();
        }

        public async Task<MatchDto> GetMatch(Guid matchStreamId)
        {
            var matchGrain = _clusterClient.GetGrain<IMatchGrain>(matchStreamId.ToString());
            var matchStateDto = await matchGrain.GetMatchState();
            return matchStateDto;
        }
    }
}