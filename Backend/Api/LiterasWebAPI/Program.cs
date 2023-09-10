using LiterasWebAPI.Config;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(new RenderedCompactJsonFormatter())
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.File(
                builder.Configuration["Serilog"],
                LogEventLevel.Warning,
                rollingInterval: RollingInterval.Hour,
                retainedFileCountLimit: 10));

    builder.Services.ConfigureServices(builder.Configuration);

    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.UseServices(builder.Configuration);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
