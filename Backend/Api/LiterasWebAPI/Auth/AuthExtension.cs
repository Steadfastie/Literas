using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace LiterasWebAPI.Auth;

public static class AuthExtension
{
    public static void AddAuth(this IServiceCollection services, IConfiguration config)
    {
        var auth0Settings = config.GetSection(nameof(Auth0Settings)).Get<Auth0Settings>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = auth0Settings.Domain;
                options.Audience = auth0Settings.Audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.LiterasRead, policy =>
            {
                policy.Requirements.Add(new HasScopeRequirement(Scopes.Read, auth0Settings.Domain));
            });

            options.AddPolicy(Policies.LiterasWrite, policy =>
            {
                policy.Requirements.Add(new HasScopeRequirement(Scopes.Write, auth0Settings.Domain));
            });

            options.AddPolicy(Policies.LiterasDelete, policy =>
            {
                policy.Requirements.Add(new HasScopeRequirement(Scopes.Delete, auth0Settings.Domain));
            });
        });
    }
}
