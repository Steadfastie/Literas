using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Queries.Contributors;

public class GetDocumentsByUserIdQuery : IRequest<IEnumerable<DocumentDto>>
{
    public Guid UserId { get; set; }
}