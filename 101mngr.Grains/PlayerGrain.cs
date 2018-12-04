using System;
using System.Threading.Tasks;
using Orleans;
using _101mngr.Contracts;

namespace _101mngr.Grains
{
    public class PlayerGrain : Grain, IPlayerGrain
    {
        public Task<long> GetPlayer()
        {
            return Task.FromResult(this.GetPrimaryKeyLong());
        }
    }
}
