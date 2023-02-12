using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Commands.Docs;

public class CreateDocCommand : IRequest<int>
{
    public DocDto Doc { get; set; }
}