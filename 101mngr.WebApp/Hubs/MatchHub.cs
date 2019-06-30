using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;

namespace _101mngr.WebApp.Hubs
{
    /// <summary>
    /// Match Hub
    /// </summary>
    public class MatchHub : Hub
    {
        private readonly MatchService _matchService;

        public MatchHub(MatchService matchService)
        {
            _matchService = matchService;
        }

        /// <summary>
        /// Get match
        /// </summary>
        /// <param name="matchStreamId"></param>
        /// <returns></returns>
        public Task<MatchDto> GetMatch(Guid matchStreamId) => _matchService.GetMatch(matchStreamId);

        /// <summary>
        /// Get match events stream
        /// </summary>
        /// <param name="matchStreamId"></param>
        /// <returns></returns>
        public Task<ChannelReader<MatchEventDto>> GetMatchStream(Guid matchStreamId)
        {
            return _matchService.GetMatchStream(matchStreamId);
        }

        /// <summary>
        /// Get current matches
        /// </summary>
        /// <returns></returns>
        public Task<MatchListItemDto[]> GetCurrentMatches()
        {
            return _matchService.GetCurrentMatches();
        }

        /// <summary>
        /// Get current matches events stream
        /// </summary>
        /// <returns></returns>
        public Task<ChannelReader<MatchListItemDto>> GetCurrentMatchesStream()
        {
            return _matchService.GetCurrentMatchesStream();
        }
    }
}