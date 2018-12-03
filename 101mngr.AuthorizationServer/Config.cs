using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace _101mngr.AuthorizationServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("web_app", "Web App", new []{JwtClaimTypes.Name, JwtClaimTypes.Email,ClaimTypes.DateOfBirth})
                {
                    ApiSecrets =
                    {
                        new Secret("secret".ToSha256())
                    }
                },
                new ApiResource("authorization_server", "Authorization Server")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "web_app.ro",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AccessTokenType = AccessTokenType.Reference,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        "web_app"
                    },
                    AccessTokenLifetime = 24 * 3600,
                },
                new Client
                {
                    ClientId = "web_app.cc",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedCorsOrigins = new List<string>() {"*"},

                    // scopes that client has access to
                    AllowedScopes = { "authorization_server" }
                }
            };
        }
    }
}