using LiterasModels.Abstractions;

namespace LiterasDataTransfer.Dto;

public class EditorDto : IBaseDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public UserDto User { get; set; }

    public Guid DocId { get; set; }
    public DocDto Doc { get; set; }
}