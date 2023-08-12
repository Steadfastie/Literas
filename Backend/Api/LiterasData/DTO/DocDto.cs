using System.Text.Json;

namespace LiterasData.DTO;

public class DocDto : IBaseDto
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public string Title { get; set; }
    public JsonDocument? TitleDelta { get; set; }
    public string Content { get; set; }
    public JsonDocument? ContentDeltas { get; set; }
}