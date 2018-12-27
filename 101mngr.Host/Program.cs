using System;
using System.Reflection;
using DbUp;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _101mngr.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();
            using (var serviceScope = webHost.Services.CreateScope())
            {
                var configuration = serviceScope.ServiceProvider.GetService<IConfiguration>();
                RunDbMigration(configuration);
            }
            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void RunDbMigration(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            EnsureDatabase.For.PostgresqlDatabase(connectionString);
            var upgradeEngine =
                DeployChanges.To
                    .PostgresqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .WithVariablesDisabled()
                    .LogToConsole()
                    .Build();

            var result = upgradeEngine.PerformUpgrade();
            if (!result.Successful)
            {
                Console.Error.WriteLine(result.Error);
                return;
            }
        }
    }
}
