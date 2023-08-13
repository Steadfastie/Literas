namespace LiterasData.DTO;

public class EditorDto : IBaseDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid DocId { get; set; }
    public DocDto Doc { get; set; }
}
