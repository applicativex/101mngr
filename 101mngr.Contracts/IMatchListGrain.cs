using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts.Models;

namespace _101mngr.Contracts
{
    public interface IMatchListGrain : IGrainWithIntegerKey
    {
        Task<MatchDto[]> GetCurrentMatches();

        Task<MatchDto[]> GetFinishedMatches();

        Task AddMatch(string matchId, TeamDto homeTeam, TeamDto awayTeam);

        Task Remove(string matchId);
    }
}