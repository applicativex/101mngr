using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;
using _101mngr.Domain;
using _101mngr.Domain.Abstractions;
using _101mngr.Domain.Enums;
using _101mngr.Domain.Events;
using _101mngr.Domain.Repositories;
using Team = _101mngr.Domain.Team;

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

        public Task AddMatchHistory(MatchDto dto)  
        {
            var match  = new Match
            {
                Id = dto.Id,
                Name = dto.Name,
                MatchPeriod = (MatchPeriod)dto.MatchPeriod,
                Minute = dto.Minute,
                StartTime = dto.StartTime,
                Goals = new Score(dto.Goals.Home, dto.Goals.Away),
                YellowCards = new Score(dto.YellowCards.Home, dto.YellowCards.Away),
                RedCards = new Score(dto.RedCards.Home, dto.RedCards.Away),
                HomeTeam = new Team
                {
                    Id = dto.HomeTeam.Id,
                    Name = dto.HomeTeam.Name
                },
                AwayTeam = new Team
                {
                    Id = dto.AwayTeam.Id,
                    Name = dto.AwayTeam.Name
                },
                MatchEvents = new List<IMatchEvent>()
            };
            return _matchHistoryRepository.AddMatch(match);
        }

        public async Task<MatchDto[]> GetPlayerMatches(long playerId)
        {
            var matches = await _matchHistoryRepository.GetMatchesByPlayer(playerId);
            return matches.Select(x => x.ToDto()).ToArray();
        }
    }
}