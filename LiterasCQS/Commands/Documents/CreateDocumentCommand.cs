using LiterasDataTransfer.DTO;
using MediatR;

namespace LiterasCQS.Commands.Documents;

public class CreateDocumentCommand : IRequest<int>
{
    public DocumentDTO Document { get; set; }
}