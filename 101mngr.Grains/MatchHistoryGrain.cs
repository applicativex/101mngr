using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;

namespace _101mngr.Grains
{
    [StatelessWorker]
    public class MatchHistoryGrain : Grain, IMatchHistoryGrain
    {
        private readonly IMatchHistoryRepository _matchHistoryRepository;

        public MatchHistoryGrain(IMatchHistoryRepository matchHistoryRepository)
        {
            _matchHistoryRepository = matchHistoryRepository;
        }

        public Task AddMatchHistory(MatchDto match)
        {
            return _matchHistoryRepository.AddMatch(match);
        }

        public Task<MatchDto[]> GetPlayerMatches(long playerId)
        {
            return _matchHistoryRepository.GetMatchesByPlayer(playerId);
        }
    }
}