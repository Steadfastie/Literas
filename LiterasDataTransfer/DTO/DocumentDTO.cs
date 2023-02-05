using LiterasModels.Abstractions;

namespace LiterasDataTransfer.Dto;

public class DocumentDto : IBaseDto
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}