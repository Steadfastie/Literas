using LiterasDataTransfer;
using LiterasDataTransfer.DTO;
using MediatR;

namespace LiterasCQS.Commands.Documents;

public class PatchDocumentCommand : IRequest<int>
{
    public DocumentDTO Document { get; set; }
    public List<PatchModel> PatchList { get; set; }
}