using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts.Models;

namespace _101mngr.Contracts
{
    public interface IMatchRegistryGrain : IGrainWithIntegerKey
    {
        Task<MatchListItemDto[]> GetCurrentMatches();

        Task<MatchListItemDto[]> GetFinishedMatches();  

        Task AddMatch(string matchId, TeamDto homeTeam, TeamDto awayTeam);

        Task RemoveMatch(string matchId);
    }
}