using LiterasData.Entities;
using LiterasModels.Abstractions;

namespace LiterasDataTransfer.Dto;

public class ContributorDto : IBaseDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid DocumentId { get; set; }
    public Document Document { get; set; }
}