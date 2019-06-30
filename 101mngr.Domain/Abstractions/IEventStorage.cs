using System.Collections.Generic;
using System.Threading.Tasks;

namespace _101mngr.Domain.Abstractions
{
    public interface IEventStorage
    {
        Task<bool> AppendToStream(string streamId, int expectedversion, params object[] updates);

        Task<KeyValuePair<int, TStreamState>> GetStreamState<TStreamState>(string streamId)
            where TStreamState : class, new();
    }
}