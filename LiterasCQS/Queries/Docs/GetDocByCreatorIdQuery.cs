using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Queries.Docs;

public class GetDocByCreatorIdQuery : IRequest<DocDto>
{
    public Guid CreatorId { get; set; }
}