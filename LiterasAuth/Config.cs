using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace LiterasAuth;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        { 
            new ApiScope("docs", "Literas docs") 
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "postman",
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "https://blog.postman.com/pkce-oauth-how-to/" },
                AllowOfflineAccess = true,
                AllowAccessTokensViaBrowser = true,
                ClientSecrets =
                {
                    new Secret("post".Sha256())
                },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "docs"
                }
            }
        };
}