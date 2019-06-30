using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;
using _101mngr.Grains;

namespace _101mngr.Host
{
    public class MatchHistoryRepository : IMatchHistoryRepository
    {
        private readonly IDocumentStore _documentStore;

        public MatchHistoryRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public async Task<MatchDto[]> GetMatchesByPlayer(long playerId)
        {
            using (var session = _documentStore.OpenSession())
            {
                var result = session.Query<MatchState>().ToArray().Select(x=>x.ToDto()).ToArray();
                return result;
            }
        }

        public Task AddMatch(MatchDto match)
        {
            using (var session = _documentStore.OpenSession())
            {
                var matchState = new MatchState
                {
                    Id = match.Id,
                    Name = match.Name,
                    MatchPeriod = match.MatchPeriod,
                    Minute = match.Minute,
                    StartTime = match.StartTime,
                    Goals = new Score(match.Goals.Home, match.Goals.Away),
                    YellowCards = new Score(match.YellowCards.Home, match.YellowCards.Away),
                    RedCards = new Score(match.RedCards.Home, match.RedCards.Away),
                    HomeTeam = new Team
                    {
                        Id = match.HomeTeam.Id,
                        Name = match.HomeTeam.Name
                    },
                    AwayTeam = new Team
                    {
                        Id = match.AwayTeam.Id,
                        Name = match.AwayTeam.Name
                    },
                    MatchEvents = new List<IMatchEvent>()
                };
                session.Store(matchState);
                session.SaveChanges();
            }
            return Task.CompletedTask;
        }
    }
}