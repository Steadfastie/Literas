using LiterasData.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiterasData;

public class NotesDBContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Contributor> Contributors { get; set; }
    public DbSet<Document> Documents { get; set; }

    public NotesDBContext(DbContextOptions<NotesDBContext> options)
    : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }
}