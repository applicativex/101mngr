using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;

namespace _101mngr.Grains
{
    public class MatchRoomRegistryGrain : Grain, IMatchRoomRegistryGrain
    {
        private readonly List<MatchRoomDto> _matchRooms = new List<MatchRoomDto>();

        public Task<MatchRoomDto[]> GetMatchRooms()
        {
            return Task.FromResult(_matchRooms.ToArray());
        }

        public Task AddMatchRoom(long playerId, string matchRoomId)
        {
            _matchRooms.Add(new MatchRoomDto
            {
                MatchId = matchRoomId,
                OwnerPlayerId = playerId.ToString(),
                Players = new MatchPlayerDto[0],
                VirtualPlayers = new MatchPlayerDto[0]
            });
            return Task.CompletedTask;
        }

        public Task RemoveMatchRoom(string matchRoomId)
        {
            var matchRoom = _matchRooms.Find(x => x.MatchId == matchRoomId);
            if (matchRoom != null)
            {
                _matchRooms.Remove(matchRoom);
            }

            return Task.CompletedTask;
        }
    }
}