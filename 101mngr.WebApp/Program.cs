using IdentityModel.Client;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace _101mngr.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var tmp = new DiscoveryClient("http://localhost:5000/").GetAsync().Result;
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
