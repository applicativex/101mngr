using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans;
using Orleans.Clustering.Kubernetes;
using _101mngr.Grains;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace _101mngr.Host
{
    public class SiloHost : IHostedService
    {
        private readonly ISiloHost _silo;

        public SiloHost(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _silo = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "101mngr";
                    options.ServiceId = "101mngr";
                })
                .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                .ConfigureClustering(hostingEnvironment)
                .ConfigureApplicationParts(parts =>
                    parts.AddApplicationPart(typeof(PlayerGrain).Assembly).WithReferences())
                .ConfigureLogging(builder => builder.AddConsole())
                .UseDashboard(options => { options.CounterUpdateIntervalMs = 10000; })
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
            else
            {
                return builder.UseKubeMembership(opt =>
                {
                    opt.Group = "orleans.dot.net";
                    opt.CanCreateResources = true;
                });
            }
        }
    }
}