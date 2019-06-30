using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using _101mngr.Contracts;

namespace _101mngr.WebApp.Hubs
{
    /// <summary>
    /// Match room Hub
    /// </summary>
    public class MatchRoomHub : Hub
    {
        private readonly MatchRoomService _matchRoomService;

        public MatchRoomHub(MatchRoomService matchRoomService)
        {
            _matchRoomService = matchRoomService;
        }

        /// <summary>
        /// Get all match rooms
        /// </summary>
        /// <returns></returns>
        public Task<MatchRoomDto[]> GetMatchRooms() => _matchRoomService.GetMatchRooms();

        /// <summary>
        /// Get match room by id
        /// </summary>
        /// <param name="matchRoomId"></param>
        /// <returns></returns>
        public Task<MatchRoomDto> GetMatchRoom(string matchRoomId)
        {
            return _matchRoomService.GetMatchRoom(matchRoomId);
        }

        /// <summary>
        /// Join match room
        /// </summary>
        /// <param name="matchRoomId"></param>
        /// <param name="playerId"></param>
        /// <param name="playerName"></param>
        /// <returns></returns>
        public Task JoinRoom(string matchRoomId, long playerId, string playerName)
        {
            return _matchRoomService.JoinRoom(matchRoomId, playerId, playerName);
        }

        /// <summary>
        /// Leave match room
        /// </summary>
        /// <param name="matchRoomId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public Task LeaveRoom(string matchRoomId, long playerId)
        {
            return _matchRoomService.LeaveRoom(matchRoomId, playerId);
        }
    }
}