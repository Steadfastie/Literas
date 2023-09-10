using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.Entities;

[Index(nameof(UserId), nameof(DocId), IsUnique = true)]
public class Editor : IBaseEntity
{
    public Guid Id { get; init; }

    [ConcurrencyCheck] 
    public int Version { get; private set; }

    public string UserId { get; init; }

    public DateTime LastContributed { get; private set; }

    public EditorStatus Status { get; init; }

    public List<EditorScope> Scopes { get; private set; }

    public Guid DocId { get; init; }

    public Doc? Doc { get; init; }

    public Editor(string userId, Guid docId, EditorStatus status, List<EditorScope> scopes)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        LastContributed = DateTime.UtcNow;
        Status = status;
        Scopes = scopes;
        DocId = docId;
    }

    public void Contribute()
    {
        LastContributed = DateTime.UtcNow;
        Version++;
    }
}
