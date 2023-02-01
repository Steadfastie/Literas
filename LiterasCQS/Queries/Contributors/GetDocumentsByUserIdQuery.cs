using LiterasDataTransfer.DTO;
using MediatR;

namespace LiterasCQS.Queries.Contributors;

public class GetDocumentsByUserIdQuery : IRequest<IEnumerable<DocumentDTO>>
{
    public Guid UserId { get; set; }
}