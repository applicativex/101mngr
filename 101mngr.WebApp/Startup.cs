using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Clustering.Kubernetes;
using Orleans.Configuration;
using Orleans.Runtime;
using Swashbuckle.AspNetCore.Swagger;
using _101mngr.Contracts;
using _101mngr.WebApp.Configuration;
using _101mngr.WebApp.Controllers;
using _101mngr.WebApp.Data;
using _101mngr.WebApp.Services;

namespace _101mngr.WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(JwtBearerDefaults.AuthenticationScheme, ConfigureAuthentication);
            services.AddSingleton<IClusterClient>(CreateClusterClient);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "101mngr API", Version = "v1" });
            });

            var authorizationServerUri = Configuration["AuthorizationServer:Authority"];
            services.Configure<AuthorizationServerOptions>(Configuration.GetSection("AuthorizationServer"));
            services.AddHttpClient<AuthorizationService>(c =>
            {
                c.BaseAddress = new Uri(authorizationServerUri);
            });

            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<MatchController.MatchRepository>();
        }
        private IClusterClient CreateClusterClient(IServiceProvider serviceProvider) =>
            StartClientWithRetries().GetAwaiter().GetResult();

        private async Task<IClusterClient> StartClientWithRetries(int initializeAttemptsBeforeFailing = 5)
        {
            int attempt = 0;
            IClusterClient client;
            while (true)
            {
                try
                {
                    client = new ClientBuilder()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "101mngr";
                            options.ServiceId = "101mngr";
                        })
                        .ConfigureClustering(HostingEnvironment)
                        .ConfigureLogging(builder => builder.AddConsole())
                        .ConfigureApplicationParts(parts =>
                            parts.AddApplicationPart(typeof(IPlayerGrain).Assembly).WithReferences())
                        .Build();
                    await client.Connect();
                    Console.WriteLine("Client successfully connect to silo host");
                    break;
                }
                catch (SiloUnavailableException)
                {
                    attempt++;
                    Console.WriteLine(
                        $"Attempt {attempt} of {initializeAttemptsBeforeFailing} failed to initialize the Orleans client.");
                    if (attempt > initializeAttemptsBeforeFailing)
                    {
                        throw;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(4));
                }
            }

            return client;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "101mngr API V1");
            });

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseMvc();
        }

        private void ConfigureAuthentication(IdentityServerAuthenticationOptions options)
        {
            var authenticationServerUrl = Configuration["AuthorizationServer:Authority"];
            options.Authority = authenticationServerUrl;
            options.RequireHttpsMetadata = false;
            options.ApiName = "web_app";
            options.ApiSecret = "secret";
            options.SupportedTokens = SupportedTokens.Reference;
        }
    }

    public static class OrleansExt
    {
        public static IClientBuilder ConfigureClustering(
            this IClientBuilder builder, IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                return builder.UseLocalhostClustering();
            }
            else
            {
                return builder.UseKubeGatewayListProvider(opt => { opt.Group = "orleans.dot.net"; });
            }
        }
    }
}
