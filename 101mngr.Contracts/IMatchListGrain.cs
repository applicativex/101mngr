using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts.Models;

namespace _101mngr.Contracts
{
    public interface IMatchListGrain : IGrainWithIntegerKey
    {
        Task<List<MatchDto>> GetMatches();

        Task Add(string id, string name);

        Task Remove(string matchId);
    }
}