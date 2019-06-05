using System;
using System.Collections.Concurrent;
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

        public ChannelReader<MatchListItemDto> GetMatchStream(Guid matchStreamId) =>
            _matchStream.GetMatchStream(matchStreamId).GetAwaiter().GetResult();
    }

    public class MatchStream
    {
        private readonly IClusterClient _clusterClient;

        public MatchStream(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public Task<ChannelReader<MatchListItemDto>> GetMatchStream(Guid matchStreamId)
        {
            var streamProvider = _clusterClient.GetStreamProvider("SMSProvider");
            var matchStream = streamProvider.GetStream<MatchListItemDto>(matchStreamId, "Matches");
            return matchStream.AsChannelReader();
        }
    }
}