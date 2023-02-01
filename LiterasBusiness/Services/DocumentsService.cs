using AutoMapper;
using LiterasCQS.Commands.Documents;
using LiterasCQS.Queries.Documents;
using LiterasDataTransfer;
using LiterasDataTransfer.DTO;
using LiterasDataTransfer.ServiceAbstractions;
using MediatR;

namespace LiterasBusiness.Services;

public class DocumentsService : IDocumentsService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    public DocumentsService(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<DocumentDTO> GetDocumentByIdAsync(Guid documentId)
    {
        if (documentId != Guid.Empty)
        {
            return await _mediator.Send(new GetDocumentByIdQuery()
            {
                Id = documentId
            });
        }
        else
        {
            throw new ArgumentException("Provided id is empty");
        }
    }

    public async Task<DocumentDTO> GetDocumentByTitleAsync(string title)
    {
        if (title != string.Empty)
        {
            return await _mediator.Send(new GetDocumentByTitleQuery()
            {
                Title = title
            });
        }
        else
        {
            throw new ArgumentException("Title is empty");
        }
    }

    public async Task<int> CreateDocumentAsync(DocumentDTO documentDTO)
    {
        if (documentDTO != null)
        {
            return await _mediator.Send(new CreateDocumentCommand()
            {
                Document = documentDTO
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(documentDTO));
        }
    }

    public async Task<int> PatchDocumentAsync(DocumentDTO documentDTO, List<PatchModel> patchlist)
    {
        var patchModelsWithId = patchlist.Where(l => l.PropertyName.Equals("Id", StringComparison.InvariantCultureIgnoreCase));

        if (patchModelsWithId.Any())
        {
            throw new ArgumentException("Id cannot be changed");
        }

        if (documentDTO != null)
        {
            return await _mediator.Send(new PatchDocumentCommand()
            {
                Document = documentDTO,
                PatchList = patchlist
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(documentDTO));
        }
    }
}
