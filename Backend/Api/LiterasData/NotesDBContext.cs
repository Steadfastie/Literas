using System;
using LiterasData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LiterasData;

public class NotesDbContext : DbContext
{
    public NotesDbContext(DbContextOptions<NotesDbContext> options)
        : base(options)
    {
    }

    public DbSet<Editor> Editors { get; set; }
    public DbSet<Doc> Docs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Editor>()
            .Property(d => d.Status)
            .HasConversion(new EnumToStringConverter<EditorStatus>());

        modelBuilder.Entity<Editor>()
            .Property(e => e.Scopes)
            .HasPostgresArrayConversion(
                v => v.ToString(),
                v => (EditorScope)Enum.Parse(typeof(EditorScope), v));

        modelBuilder.Entity<Editor>()
            .HasOne(e => e.Doc)
            .WithMany(d => d.Editors)
            .HasForeignKey(e => e.DocId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
