using Duende.IdentityServer.Services;
using IdentityModel;
using LiterasAuth.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LiterasAuth;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AuthDbContext>(optionsBuilder =>
            optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("Auth")));

        builder.Services.AddIdentity<LiterasUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "https://localhost:4800/login";

                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
            .AddAspNetIdentity<LiterasUser>()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients);

        //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        //    .AddCookie("Cookies", options =>
        //    {
        //        options.Cookie.SameSite = SameSiteMode.Lax;
        //        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //    });


        builder.Services.AddTransient<IReturnUrlParser, ReturnUrlParser>();

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseIdentityServer();

        return app;
    }
}
