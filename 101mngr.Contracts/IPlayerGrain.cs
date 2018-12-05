using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts.Models;

namespace _101mngr.Contracts
{
    public interface IPlayerGrain : IGrainWithIntegerKey
    {
        Task<long> GetPlayer();

        Task Create(CreatePlayerDto request);

        Task<PlayerDto> GetPlayerInfo();

        Task<string> NewMatch();

        Task JoinMatch(string matchId);

        Task LeaveMatch(string matchId);
    }
}
