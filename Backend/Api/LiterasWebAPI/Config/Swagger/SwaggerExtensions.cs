using IDT.Boss.AntifraudBlacklist.Api.Config.Swagger;
using LiterasWebAPI.Auth;
using LiterasWebAPI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

namespace LiterasWebAPI.Config.Swagger;

public static class SwaggerExtensions
{
    public static void AddSwaggerFeatures(this IServiceCollection services, IConfiguration config)
    {
        services.AddSwaggerGen(options =>
        {
            var settings = config.GetSection(nameof(SwaggerOAuthSettings)).Get<SwaggerOAuthSettings>();

            if (Auth0SettingsValid(settings))
            {
                throw new InvalidOperationException("Swagger configuration does not contain required fields");
            }

            var oauth2 = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                },
                OpenIdConnectUrl = new Uri(settings.OpenIdConfigurationUrl),
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(settings.OAuthAuthorizationUrl),
                        TokenUrl = new Uri(settings.OAuthTokenUrl),
                        Scopes = new Dictionary<string, string>()
                        {
                            { Scopes.OpenId, "Required to get user groups" },
                            { Scopes.Profile, "Required to get user info" },
                            { Scopes.Read, "Access read operations" },
                            { Scopes.Write, "Access write operations" },
                            { Scopes.Delete, "Access delete operation" }
                        }
                    }
                }
            };

            options.AddSecurityDefinition(oauth2.Reference.Id, oauth2);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = oauth2.Reference
                    },
                    new[] { Scopes.OpenId, Scopes.Profile, Scopes.Read, Scopes.Write, Scopes.Delete }
                }
            });

            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Literas doc WebAPI",
                    Description = "This API allows management of docs",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Alexander Divovich",
                        Email = "alex.divovich@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/alex-divovich/")
                    },
                });
            var filePath = Path.Combine(AppContext.BaseDirectory, "LiterasWebAPI.xml");
            options.IncludeXmlComments(filePath);
        });
    }

    public static void UseSwaggerFeatures(this WebApplication app, IConfiguration config)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var settings = config.GetSection(nameof(SwaggerOAuthSettings)).Get<SwaggerOAuthSettings>();

            if (Auth0SettingsValid(settings))
            {
                throw new InvalidOperationException("Swagger configuration does not contain required fields");
            }

            options.OAuthClientId(settings.OAuthClientId);
            options.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
            {
                { "audience", settings.Audience }
            });

        });
    }

    private static bool Auth0SettingsValid(SwaggerOAuthSettings settings)
    {
        return string.IsNullOrWhiteSpace(settings.Audience) || 
               string.IsNullOrWhiteSpace(settings.OAuthClientId) ||
               string.IsNullOrWhiteSpace(settings.OpenIdConfigurationUrl) ||
               string.IsNullOrWhiteSpace(settings.OAuthAuthorizationUrl) ||
               string.IsNullOrWhiteSpace(settings.OAuthTokenUrl);
    }
}
