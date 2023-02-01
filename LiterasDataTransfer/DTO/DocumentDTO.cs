namespace LiterasDataTransfer.DTO;

public class DocumentDTO
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}