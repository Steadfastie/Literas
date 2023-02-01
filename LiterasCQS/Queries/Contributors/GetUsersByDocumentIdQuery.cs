using LiterasDataTransfer.DTO;
using MediatR;

namespace LiterasCQS.Queries.Contributors;

public class GetUsersByDocumentIdQuery : IRequest<IEnumerable<UserDTO>>
{
    public Guid DocumentId { get; set; }
}