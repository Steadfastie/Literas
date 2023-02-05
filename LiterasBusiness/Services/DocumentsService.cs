using AutoMapper;
using LiterasCQS.Commands.Documents;
using LiterasCQS.Queries.Documents;
using LiterasDataTransfer.Dto;
using LiterasDataTransfer.ServiceAbstractions;
using LiterasModels.System;
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

    public async Task<DocumentDto> GetDocumentByIdAsync(Guid documentId)
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

    public async Task<DocumentDto> GetDocumentByTitleAsync(string title)
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

    public async Task<int> CreateDocumentAsync(DocumentDto documentDto)
    {
        if (documentDto != null)
        {
            return await _mediator.Send(new CreateDocumentCommand()
            {
                Document = documentDto
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(documentDto));
        }
    }

    public async Task<int> PatchDocumentAsync(DocumentDto documentDto, List<PatchModel> patchlist)
    {
        var patchModelsWithId = patchlist
            .Where(l => l.PropertyName
                .Equals("Id", StringComparison.InvariantCultureIgnoreCase));

        if (patchModelsWithId.Any())
        {
            throw new ArgumentException("Id cannot be changed");
        }

        if (documentDto != null)
        {
            return await _mediator.Send(new PatchDocumentCommand()
            {
                Document = documentDto,
                PatchList = patchlist
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(documentDto));
        }
    }

    public async Task<int> DeleteDocumentAsync(DocumentDto documentDto)
    {
        if (documentDto != null)
        {
            return await _mediator.Send(new DeleteDocumentCommand()
            {
                Document = documentDto
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(documentDto));
        }
    }
}