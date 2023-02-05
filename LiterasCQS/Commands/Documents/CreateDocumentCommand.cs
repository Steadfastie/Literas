using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Commands.Documents;

public class CreateDocumentCommand : IRequest<int>
{
    public DocumentDto Document { get; set; }
}