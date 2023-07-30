using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Commands.Docs;

public class DeleteDocCommand : IRequest<int>
{
    public DocDto Doc { get; set; }
}