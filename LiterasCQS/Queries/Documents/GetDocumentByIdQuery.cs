using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Queries.Documents;

public class GetDocumentByIdQuery : IRequest<DocumentDto>
{
    public Guid Id { get; set; }
}