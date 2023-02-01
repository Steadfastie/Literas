using LiterasDataTransfer.DTO;
using MediatR;

namespace LiterasCQS.Queries.Documents;

public class GetDocumentByIdQuery : IRequest<DocumentDTO>
{
    public Guid Id { get; set; }
}