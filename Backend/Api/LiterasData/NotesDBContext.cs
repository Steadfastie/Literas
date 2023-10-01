using LiterasData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
                v => (EditorScope)Enum.Parse(typeof(EditorScope), v))
            .Metadata.SetValueComparer(new ValueComparer<List<EditorScope>>(
                (c1, c2) => c2 != null && c1 != null && c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        modelBuilder.Entity<Editor>()
            .HasOne(e => e.Doc)
            .WithMany(d => d.Editors)
            .HasForeignKey(e => e.DocId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
