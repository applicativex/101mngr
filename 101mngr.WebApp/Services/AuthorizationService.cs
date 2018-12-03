using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using _101mngr.WebApp.Configuration;

namespace _101mngr.WebApp.Services
{
    public class AuthorizationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthorizationService> _logger;
        private readonly string _authorizationServerUrl;
        private DiscoveryResponse _discoveryDocument;

        public AuthorizationService(
            HttpClient httpClient, IOptions<AuthorizationServerOptions> authorizationServerOptions,
            ILogger<AuthorizationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _authorizationServerUrl = authorizationServerOptions.Value.Authority;
        }

        public async Task<string> Register(string userName, string email, string password, string countryCode)
        {
            var token = await GetClientCredentialsToken();
            _httpClient.SetBearerToken(token.AccessToken);
            var response = await _httpClient.PostAsync("/api/account/register",
                new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        Email = email,
                        UserName = userName,
                        Password = password,
                        CountryCode = countryCode
                    }), Encoding.UTF8,
                    "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<string> Login(string login, string password)
        {
            var userName = login.Contains('@') ? await GetUserName(email: login) : login;
            var token = await GetResourceOwnerToken(userName, password);
            if (token.IsError)
            {
                throw new Exception(token.Error);
            }
            return token.AccessToken;

            async Task<string> GetUserName(string email)
            {
                var clientCredentialsToken = await GetClientCredentialsToken();
                _httpClient.SetBearerToken(clientCredentialsToken.AccessToken);
                var response = await _httpClient.GetAsync($"api/account/username/{email}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }

        public async Task<string> GetUserInfo(long id)
        {
            var token = await GetClientCredentialsToken();
            _httpClient.SetBearerToken(token.AccessToken);
            var response = await _httpClient.GetAsync($"api/account/{id}/userinfo");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task Logout(string token)
        {
            var discoveryDocument = await GetDiscoveryDocument();
            using (var revocationClient = new TokenRevocationClient(discoveryDocument.RevocationEndpoint,
                clientId: "web_app.ro", clientSecret: "secret"))
            {
                var response = await revocationClient.RevokeAccessTokenAsync(token);
            }
        }

        private async Task<TokenResponse> GetClientCredentialsToken()
        {
            var discoveryDocument = await GetDiscoveryDocument();
            using (var tokenClient =
                new TokenClient(discoveryDocument.TokenEndpoint, clientId: "web_app.cc", clientSecret: "secret"))
            {
                var tokenResponse = await tokenClient.RequestClientCredentialsAsync(scope: "authorization_server");
                if (tokenResponse.IsError)
                {
                    _logger.LogError(tokenResponse.Error, "Failed get token.");
                    throw new Exception(tokenResponse.Error);
                }

                return tokenResponse;
            }
        }

        private async Task<TokenResponse> GetResourceOwnerToken(string email, string password)
        {
            var discoveryDocument = await GetDiscoveryDocument();
            using (var tokenClient =
                new TokenClient(discoveryDocument.TokenEndpoint, clientId: "web_app.ro", clientSecret: "secret"))
            {
                var tokenResponse =
                    await tokenClient.RequestResourceOwnerPasswordAsync(
                        userName: email, password: password, scope:"web_app");
                if (tokenResponse.IsError)
                {
                    _logger.LogError(tokenResponse.Error, "Failed get token.");
                    throw new Exception(tokenResponse.Error);
                }

                return tokenResponse;
            }
        }

        private async ValueTask<DiscoveryResponse> GetDiscoveryDocument()
        {
            return _discoveryDocument ??
                   (_discoveryDocument = await new DiscoveryClient(_authorizationServerUrl).GetAsync());
        }
    }
}