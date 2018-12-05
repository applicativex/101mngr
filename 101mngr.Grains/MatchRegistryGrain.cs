using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;
using _101mngr.Contracts.Models;

namespace _101mngr.Grains
{
    public class MatchRegistryGrain : Grain, IMatchRegistryGrain
    {
        private List<MatchDto> _matchList;

        public override Task OnActivateAsync()
        {
            _matchList = new List<MatchDto>();
            return base.OnActivateAsync();
        }

        public Task<List<MatchDto>> GetMatches()
        {
            return Task.FromResult(_matchList);
        }

        public Task Register(MatchDto dto)
        {
            _matchList.Add(new MatchDto());
            return Task.CompletedTask;
        }

        public Task Remove(string matchId)
        {
            _matchList.RemoveAll(x => x.Id == matchId);
            return Task.CompletedTask;
        }
    }
}