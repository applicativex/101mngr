using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts.Models;

namespace _101mngr.Contracts
{
    public interface IMatchGrain : IGrainWithStringKey
    {
        Task NewMatch(PlayerDataDto player);

        Task JoinMatch(PlayerDataDto player);

        // Task LeaveMatch(long playerId);
            
        Task PickCaptains();

        Task PickPlayer(int team, long playerId);

        Task<PlayerDataDto[]> GetPlayers();

        Task StartMatch();
    }
}