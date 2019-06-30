using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts.Models;

namespace _101mngr.Contracts
{
    public interface IMatchHistoryGrain : IGrainWithIntegerKey
    {
        Task AddMatchHistory(MatchDto match);

        Task<MatchDto[]> GetPlayerMatches(long playerId);
    }
}