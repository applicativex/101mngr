using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using _101mngr.Contracts;

namespace _101mngr.WebApp.Hubs
{
    public class MatchRoomHub : Hub
    {
        private readonly MatchRoomService _matchRoomService;

        public MatchRoomHub(MatchRoomService matchRoomService)
        {
            _matchRoomService = matchRoomService;
        }

        public Task<MatchRoomDto[]> GetMatchRooms() => _matchRoomService.GetMatchRooms();

        public Task<MatchRoomDto> GetMatchRoom(string matchRoomId)
        {
            return _matchRoomService.GetMatchRoom(matchRoomId);
        }

        public Task JoinRoom(string matchRoomId, long playerId, string playerName)
        {
            return _matchRoomService.JoinRoom(matchRoomId, playerId, playerName);
        }

        public Task LeaveRoom(string matchRoomId, long playerId)
        {
            return _matchRoomService.LeaveRoom(matchRoomId, playerId);
        }
    }
}