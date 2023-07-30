using LiterasDataTransfer.Dto;
using LiterasModels.System;
using MediatR;

namespace LiterasCQS.Commands.Docs;

public class PatchDocCommand : IRequest<int>
{
    public DocDto Doc { get; set; }
    public List<PatchModel> PatchList { get; set; }
}