using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans;
using _101mngr.Grains;

namespace _101mngr.Host
{
    public class SiloHost : IHostedService
    {
        private readonly ISiloHost _silo;

        public SiloHost(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _silo = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "wallet";
                    options.ServiceId = "wallet";
                })
                .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                .UseLocalhostClustering()
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(PlayerGrain).Assembly).WithReferences())
                .ConfigureLogging(builder => builder.AddConsole())
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
}