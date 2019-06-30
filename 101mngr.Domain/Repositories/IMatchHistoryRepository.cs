using System.Collections.Generic;
using System.Threading.Tasks;

namespace _101mngr.Domain.Repositories
{
    public interface IMatchHistoryRepository
    {
        Task<IReadOnlyList<Match>> GetMatchesByPlayer(long playerId);
        Task AddMatch(Match match);
    }
}