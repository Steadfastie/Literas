using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Commands.Documents;

public class DeleteDocumentCommand : IRequest<int>
{
    public DocumentDto Document { get; set; }
}