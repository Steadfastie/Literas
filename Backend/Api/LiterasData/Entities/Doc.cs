using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;

namespace LiterasData.Entities;

public class Doc
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public string Title { get; set; }
    public JsonDocument? TitleDelta { get; set; }

    public string Content { get; set; }
    public JsonDocument? ContentDeltas { get; set; }
}