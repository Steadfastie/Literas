using LiterasOAuth;
using Microsoft.AspNetCore.CookiePolicy;
using Serilog;
using Serilog.Events;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();

    builder.Host.UseSerilog((ctx, lc) =>
        lc.WriteTo.File(
                builder.Configuration["Serilog"],
                LogEventLevel.Warning,
                rollingInterval: RollingInterval.Hour,
                retainedFileCountLimit: 10)
            .WriteTo.Console(LogEventLevel.Information));

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("https://localhost:4200", "https://localhost:7034");
        });
        options.AddPolicy(
            name: "literas",
            policy =>
            {
                policy.WithOrigins(
                        "https://localhost:4800")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    app.UseCors();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHsts();
    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}
catch (Exception ex) when (
    ex.GetType().Name is not "StopTheHostException"
    && ex.GetType().Name is not "HostAbortedException")
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

