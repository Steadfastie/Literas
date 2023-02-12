using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Queries.Editors;

public class GetDocsByUserIdQuery : IRequest<IEnumerable<DocDto>>
{
    public Guid UserId { get; set; }
}