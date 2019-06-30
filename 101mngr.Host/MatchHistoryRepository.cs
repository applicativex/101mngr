using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using _101mngr.Domain;
using _101mngr.Domain.Repositories;

namespace _101mngr.Host
{
    public class MatchHistoryRepository : IMatchHistoryRepository
    {
        private readonly IDocumentStore _documentStore;

        public MatchHistoryRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task<IReadOnlyList<Match>> GetMatchesByPlayer(long playerId)
        {
            using (var session = _documentStore.OpenSession())
            {
                var result = session.Query<Match>().ToArray();
                return result;
            }
        }

        public Task AddMatch(Match match)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(match);
                session.SaveChanges();
            }
            return Task.CompletedTask;
        }
    }
}