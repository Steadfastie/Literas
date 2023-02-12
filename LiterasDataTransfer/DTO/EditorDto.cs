using LiterasData.Entities;
using LiterasModels.Abstractions;

namespace LiterasDataTransfer.Dto;

public class EditorDto : IBaseDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid DocId { get; set; }
    public Doc Doc { get; set; }
}