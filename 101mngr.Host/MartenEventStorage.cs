using System.Collections.Generic;
using System.Threading.Tasks;
using Marten;
using Microsoft.Extensions.Logging;
using _101mngr.Contracts;
using _101mngr.Domain;
using _101mngr.Domain.Abstractions;

namespace _101mngr.Host
{
    public class MartenEventStorage : IEventStorage
    {
        private readonly IDocumentStore _documentStore;
        private readonly ILogger<IEventStorage> _logger;

        public MartenEventStorage(IDocumentStore documentStore, ILogger<IEventStorage> logger)
        {
            _documentStore = documentStore;
            _logger = logger;
        }

        public async Task<bool> AppendToStream(string streamId, int expectedversion, params object[] updates)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Events.Append(streamId, expectedversion, updates);
                session.SaveChanges();
            }

            return true;
        }

        public async Task<KeyValuePair<int, TStreamState>> GetStreamState<TStreamState>(string streamId) where TStreamState : class, new()
        {
            int version;
            TStreamState stream;
            using (var session = _documentStore.OpenSession())
            {
                var streamState = session.Events.FetchStreamState(streamId);
                version = streamState?.Version ?? 0;
                if (version != 0)
                {
                    stream = session.Events.AggregateStream<TStreamState>(streamId);// session.Load<TStreamState>(streamId);
                }
                else
                {
                    stream = new TStreamState();
                }
            }

            return new KeyValuePair<int, TStreamState>(version, stream);
        }
    }
}