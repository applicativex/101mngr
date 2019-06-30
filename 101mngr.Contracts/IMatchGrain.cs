using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts.Models;

namespace _101mngr.Contracts
{
    public interface IMatchGrain : IGrainWithStringKey
    {
        Task<MatchDto> GetMatchState();

        Task Start(TeamDto homeTeam, TeamDto awayTeam);

        Task FinishMatch();
    }
}