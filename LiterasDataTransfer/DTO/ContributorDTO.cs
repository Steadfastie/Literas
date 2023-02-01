using LiterasData.Entities;

namespace LiterasDataTransfer.DTO;

public class ContributorDTO
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid DocumentId { get; set; }
    public Document Document { get; set; }
}