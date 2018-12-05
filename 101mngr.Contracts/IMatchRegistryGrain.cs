using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts.Models;

namespace _101mngr.Contracts
{
    public interface IMatchRegistryGrain : IGrainWithIntegerKey
    {
        Task<List<MatchDto>> GetMatches();

        Task Register(MatchDto dto);

        Task Remove(string matchId);
    }
}