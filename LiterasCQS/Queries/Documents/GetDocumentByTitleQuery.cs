using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Queries.Documents;

public class GetDocumentByTitleQuery : IRequest<DocumentDto>
{
    public string Title { get; set; }
}