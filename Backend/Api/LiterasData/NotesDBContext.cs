using LiterasData.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiterasData;

public class NotesDBContext : DbContext
{
    public NotesDBContext(DbContextOptions<NotesDBContext> options)
        : base(options)
    {
    }

    public DbSet<Editor> Editors { get; set; }
    public DbSet<Doc> Docs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }
}
