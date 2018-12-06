using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts.Models;

namespace _101mngr.Contracts
{
    public interface IMatchGrain : IGrainWithStringKey
    {
        Task<MatchInfoDto> GetMatchInfo();

        Task NewMatch(long playerId, string playerName, string matchName);

        Task JoinMatch(long playerId, string playerName);

        Task LeaveMatch(long playerId);

        Task PlayMatch();  
    }
}