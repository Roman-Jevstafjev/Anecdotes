using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using System.Security.Claims;

namespace Jevstafjev.Anecdotes.Web.Definitions.IdentityServer
{
    public class IdentityServerConfiguration
    {
        public static IEnumerable<Client> GetClients()
        {
            yield return new Client
            {
                ClientId = "client-code-id",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequireConsent = false,
                RequirePkce = true,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Address,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "api"
                },
                RedirectUris =
                {
                    "https://localhost:10001/swagger/oauth2-redirect.html",
                    "https://localhost:5001/authentication/login-callback",
                    "https://www.thunderclient.com/oauth/callback"
                },
                PostLogoutRedirectUris = { "https://localhost:5001/authentication/logout-callback" },
                AllowedCorsOrigins = { "https://localhost:5001" }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            yield return new ApiResource("api");
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            yield return new IdentityResources.OpenId();
            yield return new IdentityResources.Address();
            yield return new IdentityResources.Profile();
            yield return new IdentityResources.Email();
        }

        public static IEnumerable<ApiScope> GetScopes()
        {
            yield return new ApiScope("api");
        }
    }
}
