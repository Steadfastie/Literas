using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Queries.Docs;

public class GetDocByTitleQuery : IRequest<DocDto>
{
    public string Title { get; set; }
}