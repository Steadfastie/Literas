using LiterasAuth.Auth;
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
                options.UserInteraction.LoginUrl = "http://localhost:4800/login";
                options.UserInteraction.LogoutUrl = "http://localhost:4800/logout";

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

        //builder.Services.ConfigureApplicationCookie(options =>
        //{
        //    options.LoginPath = "localhost:4800/login";
        //});

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseIdentityServer();

        return app;
    }
}
