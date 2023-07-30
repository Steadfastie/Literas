using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Queries.Docs;

public class GetDocByIdQuery : IRequest<DocDto>
{
    public Guid Id { get; set; }
}