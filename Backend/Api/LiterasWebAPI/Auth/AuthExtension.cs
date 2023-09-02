using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace LiterasWebAPI.Auth;

public static class AuthExtension
{
    public static void AddAuth(this IServiceCollection services, IConfiguration config)
    {
        var authority = config.Get<AuthConfig>().Authority;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                options.Authority = authority;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidIssuer = authority,
                    ClockSkew = TimeSpan.Zero,
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.LiterasRead, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", LiterasScopes.Read);
            });

            options.AddPolicy(Policies.LiterasWrite, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", LiterasScopes.Write);
            });

            options.AddPolicy(Policies.LiterasDelete, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", LiterasScopes.Delete);
            });
        });
    }
}
