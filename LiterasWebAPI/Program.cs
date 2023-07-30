using LiterasBusiness.Services;
using LiterasCQS.Queries.Users;
using LiterasData;
using LiterasDataTransfer.ServiceAbstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using LiterasWebAPI.Auth;

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

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
        options.Authority = "https://localhost:7034";
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidIssuer = "https://localhost:7034",
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("literas", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "docs");
    });
});

builder.Services.AddDbContext<NotesDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DocsPostgre")));

builder.Services.AddAutoMapper(Assembly.Load("LiterasDataTransfer"));

builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformator>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();