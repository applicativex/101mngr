using System.Threading.Tasks;
using Orleans;

namespace _101mngr.Contracts
{
    public interface IMatchRoomRegistryGrain : IGrainWithIntegerKey
    {
        Task<MatchRoomDto[]> GetMatchRooms();

        Task AddMatchRoom(long playerId, string matchRoomId);

        Task RemoveMatchRoom(string matchRoomId);
    }
}   