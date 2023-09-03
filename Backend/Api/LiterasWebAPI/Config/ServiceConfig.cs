using LiterasCore.Abstractions;
using LiterasCore.Services;
using LiterasData;
using LiterasData.CQS;
using LiterasData.DTO;
using LiterasWebAPI.Auth;
using LiterasWebAPI.Config.MappingProfiles;
using LiterasWebAPI.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace LiterasWebAPI.Config;

public static class ServiceConfig
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration config)
    {
        var cors = config.GetSection(nameof(Cors)).Get<Cors>();
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(cors.Origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        services.AddAuth(config);

        services.AddDbContext<NotesDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DocsPostgres")));

        services.AddAutoMapper(typeof(DocDto).Assembly);
        services.AddAutoMapper(typeof(DocsService).Assembly);
        services.AddAutoMapper(typeof(DocsProfile).Assembly);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IDocsService>());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RetryPolicyBehavior<,>));

        services.AddServices();

        services.AddControllers(opt =>
        {
            opt.Filters.Add<GlobalExceptionFilter>();
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public static void UseServices(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<UserInfoExtractorMiddleware>();
        app.UseMiddleware<LoggerEnricherMiddleware>();
        app.MapControllers();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IClaimsTransformation, ClaimsTransformator>();
        services.AddScoped<IDocsService, DocsService>();
        services.AddScoped<IEditorsService, EditorsService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddSingleton<IEventBus, RabbitMQService>();
    }
}
