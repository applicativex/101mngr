using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Marten.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans;
using Orleans.Clustering.Kubernetes;
using _101mngr.Contracts;
using _101mngr.Domain;
using _101mngr.Domain.Abstractions;
using _101mngr.Domain.Events;
using _101mngr.Domain.Repositories;
using _101mngr.Grains;
using _101mngr.Leagues;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace _101mngr.Host
{
    public class SiloHost : IHostedService
    {
        private readonly ISiloHost _silo;

        public SiloHost(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _silo = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "101mngr";
                    options.ServiceId = "101mngr";
                })
                .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                .ConfigureServices(services =>
                {
                    var documentStore = DocumentStore.For(_ =>
                    {
                        _.Connection(connectionString);

                        _.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
                        _.Events.StreamIdentity = StreamIdentity.AsString;
                        _.Events.AddEventTypes(new[]
                        {
                            typeof(PlayerCreated),
                            typeof(ProfileInfoChanged)
                        });
                        // _.Events.InlineProjections.AggregateStreamsWith<PlayerState>();
                    });
                    services.AddSingleton<IDocumentStore>(documentStore);
                    services.AddSingleton<IEventStorage, MartenEventStorage>();
                    services.AddSingleton<LeagueDbContext>();
                    services.AddSingleton<LeagueService>();
                    services.AddSingleton<IMatchHistoryRepository, MatchHistoryRepository>();
                })
                .ConfigureClustering(hostingEnvironment)
                .ConfigureApplicationParts(parts =>
                    parts.AddApplicationPart(typeof(PlayerGrain).Assembly).WithReferences())
                .ConfigureLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Warning))
                .UseDashboard(options => { options.CounterUpdateIntervalMs = 10000; })
                .AddSimpleMessageStreamProvider("SMSProvider")
                .AddMemoryGrainStorage("PubSubStore")
                .Build();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _silo.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _silo.StopAsync(cancellationToken);
        }
    }

    public static class OrleansExt
    {
        public static ISiloHostBuilder ConfigureClustering(
            this ISiloHostBuilder builder, IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                return builder.UseLocalhostClustering();
            }
            else if (hostingEnvironment.IsEnvironment("Compose"))
            {
                return builder.UseComposeClustering("host");
            }
            else
            {
                return builder.UseKubeMembership(opt =>
                {
                    opt.Group = "orleans.dot.net";
                    opt.CanCreateResources = true;
                });
            }
        }

        public static ISiloHostBuilder UseComposeClustering(this ISiloHostBuilder builder, string siloServiceName)
        {
            return UseIntegrationClusteringImpl(
                address: Dns.GetHostEntry(siloServiceName).AddressList[0],
                siloPort: EndpointOptions.DEFAULT_SILO_PORT,
                gatewayPort: EndpointOptions.DEFAULT_GATEWAY_PORT);

            ISiloHostBuilder UseIntegrationClusteringImpl(IPAddress address, int siloPort, int gatewayPort)
            {
                return builder.UseLocalhostClustering(primarySiloEndpoint: new IPEndPoint(address, siloPort))
                    .ConfigureEndpoints(siloPort: siloPort, gatewayPort: gatewayPort, advertisedIP: address);
            }
        }
    }
}