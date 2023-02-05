using LiterasDataTransfer.Dto;
using LiterasModels.System;
using MediatR;

namespace LiterasCQS.Commands.Documents;

public class PatchDocumentCommand : IRequest<int>
{
    public DocumentDto Document { get; set; }
    public List<PatchModel> PatchList { get; set; }
}