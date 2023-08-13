using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace LiterasData.Entities;

public class Doc : IBaseEntity
{
    public Guid Id { get; set; }
    [ConcurrencyCheck]
    public int Version { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Title { get; set; }
    public JsonDocument? TitleDelta { get; set; }
    public string Content { get; set; }
    public JsonDocument? ContentDeltas { get; set; }
}
