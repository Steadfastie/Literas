using AutoMapper;
using LiterasCQS.Commands.Docs;
using LiterasCQS.Queries.Docs;
using LiterasDataTransfer.Dto;
using LiterasDataTransfer.ServiceAbstractions;
using LiterasModels.System;
using MediatR;

namespace LiterasBusiness.Services;

public class DocsService : IDocsService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public DocsService(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<CrudResult<DocDto>> GetDocByIdAsync(Guid docId)
    {
        if (docId == Guid.Empty)
        {
            throw new ArgumentException(
                $"Provided ID (..{docId.ToString()[^5..]}) is empty");
        }

        var docFromDb = await _mediator.Send(new GetDocByIdQuery()
        {
            Id = docId
        });

        return docFromDb != null
            ? new CrudResult<DocDto>(docFromDb)
            : new CrudResult<DocDto>();
    }

    public async Task<CrudResults<IEnumerable<DocDto>>> GetDocThumbnailsAsync()
    {
        var docThumbnails = await _mediator.Send(new GetDocThumbnailsQuery());

        return docThumbnails != null
            ? new CrudResults<IEnumerable<DocDto>>(docThumbnails)
            : new CrudResults<IEnumerable<DocDto>>();
    }

    public async Task<CrudResult<DocDto>> GetDocByCreatorIdAsync(Guid creatorId)
    {
        if (creatorId != Guid.Empty)
        {
            throw new ArgumentException(
                $"Provided ID (..{creatorId.ToString()[^5..]}) is empty");
        }

        var docFromDb = await _mediator.Send(new GetDocByCreatorIdQuery()
        {
            CreatorId = creatorId
        });

        return docFromDb != null
            ? new CrudResult<DocDto>(docFromDb)
            : new CrudResult<DocDto>();
    }

    public async Task<CrudResult<DocDto>> GetDocByTitleAsync(string title)
    {
        if (title != string.Empty) throw new ArgumentException("Title is empty");

        var docFromDb = await _mediator.Send(new GetDocByTitleQuery()
        {
            Title = title
        });

        return docFromDb != null
            ? new CrudResult<DocDto>(docFromDb)
            : new CrudResult<DocDto>();
    }

    public async Task<CrudResult<DocDto>> CreateDocAsync(DocDto docDto)
    {
        if (docDto.Id == Guid.Empty)
        {
            docDto.Id = Guid.NewGuid();
        }

        if (docDto.CreatorId == Guid.Empty)
        {
            throw new ArgumentException(
                $"Creator ID (..{docDto.CreatorId.ToString()[^5..]}) can't be empty");
        }

        var presenceCheck = await _mediator.Send(new GetDocByIdQuery() { Id = docDto.Id });
        if (presenceCheck is not null)
        {
            return new CrudResult<DocDto>();
        }

        var saveChangesResult = await _mediator.Send(new CreateDocCommand()
        {
            Doc = docDto
        });

        return saveChangesResult == 1
            ? new CrudResult<DocDto>(docDto)
            : new CrudResult<DocDto>();
    }

    public async Task<CrudResult<DocDto>> PatchDocAsync(Guid docId, DocDto docDto)
    {
        var sourceDto = await _mediator.Send(new GetDocByIdQuery() { Id = docId });
        if (sourceDto == null)
        {
            throw new ArgumentException(
                $"Found no doc with provided ID (..{docId.ToString()[^5..]})");
        }

        var ignoreFields = new[]
        {
            docDto.GetType().GetProperty("Id")!,
            docDto.GetType().GetProperty("CreatorId")!,
        };

        var patchList = PatchModelCreator<DocDto>.Generate(sourceDto, docDto, ignoreFields);

        if (!patchList.Any())
        {
            return new CrudResult<DocDto>();
        }

        var saveChangesResult = await _mediator.Send(new PatchDocCommand()
        {
            Doc = sourceDto,
            PatchList = patchList
        });

        if (saveChangesResult == 0)
        {
            return new CrudResult<DocDto>();
        }

        var updatedDto = await _mediator.Send(new GetDocByIdQuery() { Id = docId });

        return new CrudResult<DocDto>(updatedDto);
    }

    public async Task<CrudResult<DocDto>> DeleteDocAsync(Guid docId)
    {
        var sourceDto = await _mediator.Send(new GetDocByIdQuery() { Id = docId });
        if (sourceDto == null)
        {
            throw new ArgumentException(
                $"Found no doc with provided ID (..{docId.ToString()[^5..]})", nameof(docId));
        }

        var saveChangesResult = await _mediator.Send(new DeleteDocCommand()
        {
            Doc = sourceDto
        });

        return saveChangesResult == 1
            ? new CrudResult<DocDto>(sourceDto)
            : new CrudResult<DocDto>();
    }
}