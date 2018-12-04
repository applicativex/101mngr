using System;
using System.Threading.Tasks;
using Orleans;

namespace _101mngr.Contracts
{
    public interface IPlayerGrain : IGrainWithIntegerKey
    {
        Task<long> GetPlayer(); 
    }
}
