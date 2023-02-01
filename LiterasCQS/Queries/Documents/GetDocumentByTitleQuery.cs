using LiterasDataTransfer.DTO;
using MediatR;

namespace LiterasCQS.Queries.Documents;

public class GetDocumentByTitleQuery : IRequest<DocumentDTO>
{
    public string Title { get; set; }
}