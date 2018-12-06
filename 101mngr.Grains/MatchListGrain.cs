using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;

namespace _101mngr.Grains
{
    public class MatchListGrain : Grain, IMatchListGrain
    {
        private List<MatchDto> _matchList;

        public override Task OnActivateAsync()
        {
            _matchList = new List<MatchDto>();
            return base.OnActivateAsync();
        }

        public Task<List<MatchDto>> GetMatches()
        {
            return Task.FromResult(_matchList.OrderByDescending(x => x.CreatedAt).ToList());
        }

        public Task Add(string id, string name)
        {
            _matchList.Add(new MatchDto
            {
                Id = id,
                Name = name,
                CreatedAt = DateTime.UtcNow
            });
            return Task.CompletedTask;
        }

        public Task Remove(string matchId)
        {
            _matchList.RemoveAll(x => x.Id == matchId);
            return Task.CompletedTask;
        }
    }
}