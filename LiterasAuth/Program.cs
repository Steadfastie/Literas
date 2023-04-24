using LiterasAuth;
using Serilog;
using Serilog.Events;

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
    options.AddPolicy(
        name: "literas",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:4800")
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
