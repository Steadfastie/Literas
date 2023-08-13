using LiterasData.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiterasData;

public class NotesDBContext : DbContext
{
    public DbSet<Editor> Editors { get; set; }
    public DbSet<Doc> Docs { get; set; }

    public NotesDBContext(DbContextOptions<NotesDBContext> options)
    : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }
}
