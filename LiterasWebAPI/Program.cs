using LiterasBusiness.Services;
using LiterasCQS.Queries.Users;
using LiterasData;
using LiterasDataTransfer.ServiceAbstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();

    }); 
    options.AddPolicy(
        name: "literas",
        policy =>
        {
            policy.WithOrigins(
                    "https://localhost:4800",
                    "https://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Host.UseSerilog((ctx, lc) =>
        lc.WriteTo.File(
            builder.Configuration["Serilog"],
            LogEventLevel.Warning,
            rollingInterval: RollingInterval.Hour,
            retainedFileCountLimit: 10)
            .WriteTo.Console(LogEventLevel.Information));

builder.Services.AddDbContext<NotesDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DocsPostgre")));

builder.Services.AddAutoMapper(Assembly.Load("LiterasDataTransfer"));

builder.Services.AddScoped<IDocsService, DocsService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IEditorsService, EditorsService>();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(typeof(GetUserByIdQuery).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();