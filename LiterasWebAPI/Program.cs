using LiterasBusiness.Services;
using LiterasData;
using LiterasDataTransfer.ServiceAbstractions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NotesDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("NotesPostgre")));

builder.Services.AddAutoMapper(Assembly.Load("LiterasDataTransfer"));

builder.Services.AddScoped<IDocumentsService, DocumentsService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IContributorsService, ContributorsService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();