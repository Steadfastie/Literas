using LiterasData.Entities;

namespace LiterasData.DTO;

public class EditorDto : IBaseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public Guid DocId { get; set; }
    public DateTime LastContributed { get; set; }

    public EditorStatus Status { get; init; }

    public List<EditorScope> Scopes { get; set; }
}
