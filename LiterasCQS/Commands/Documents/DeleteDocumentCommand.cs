using LiterasDataTransfer.DTO;
using MediatR;

namespace LiterasCQS.Commands.Documents;

public class DeleteDocumentCommand : IRequest<int>
{
    public DocumentDTO Document { get; set; }
}