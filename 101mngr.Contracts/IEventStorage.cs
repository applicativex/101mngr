using System.Collections.Generic;
using System.Threading.Tasks;

namespace _101mngr.Contracts
{
    public interface IEventStorage
    {
        Task<bool> AppendToStream(string streamId, int expectedversion, params object[] updates);

        Task<KeyValuePair<int, TStreamState>> GetStreamState<TStreamState>(string streamId)
            where TStreamState : class, new();
    }
}