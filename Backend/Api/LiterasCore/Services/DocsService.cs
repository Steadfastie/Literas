using AutoMapper;
using LiterasCore.Abstractions;
using LiterasCore.System;
using LiterasData.CQS;
using LiterasData.CQS.Queries;
using LiterasData.DTO;
using LiterasData.Entities;
using LiterasData.Exceptions;
using MediatR;

namespace LiterasCore.Services;

public class DocsService : IDocsService
{
    private readonly IMediator _mediator;
    private readonly IIdentityService _identityService;
    private readonly IEditorsService _editorsService;
    private readonly IEventBus _notificationService;
    private readonly IMapper _mapper;

    public DocsService(IMediator mediator, 
        IMapper mapper, 
        IIdentityService identityService, 
        IEditorsService editorsService, 
        IEventBus notificationService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _identityService = identityService;
        _editorsService = editorsService;
        _notificationService = notificationService;
    }

    public async Task<CrudResult<DocDto>> GetDocByIdAsync(Guid docId)
    {
        if (docId == Guid.Empty)
        {
            throw new ArgumentException(
                $"Provided ID (..{docId.ToString()[^5..]}) is empty");
        }

        var docFromDb = await _mediator.Send(new GetEditorScopes()
        {
            DocId = docId
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

    public async Task<Guid> CreateDocAsync(DocDto newDoc)
    {
        var doc = new Doc(newDoc.Id,
            (newDoc.Title, newDoc.TitleDelta),
            (newDoc.Content, newDoc.ContentDelta));

        var creator = new Editor(_identityService.UserId, newDoc.Id, EditorStatus.Creator,
            new List<EditorScope> { EditorScope.CanRead, EditorScope.CanWrite, EditorScope.CanDelete });

        var saveChangesResult = await _mediator.Send(new CreateDocCommand()
        {
            Doc = doc,
            Creator = creator
        });

        if (saveChangesResult != 1)
        {
            throw new GeneralException("Doc creation went wrong");
        }

        return doc.Id;
    }

    public async Task PatchDocAsync(DocDto changedDocDto)
    {
        var canUserEdit = await _editorsService.CanUserDo(changedDocDto.Id,
            new List<EditorScope>() { EditorScope.CanRead, EditorScope.CanWrite });

        if (!canUserEdit)
        {
            throw new ForbiddenException("User don't have enough rights to do that");
        }

        var changedDoc = new Doc(changedDocDto.Id,
            (changedDocDto.Title, changedDocDto.TitleDelta),
            (changedDocDto.Content, changedDocDto.ContentDelta));

        var docAfter = await _mediator.Send(new GetAndPatchDocCommand()
        {
            Doc = changedDoc,
            UserId = _identityService.UserId
        });

        var docToTransfer = _mapper.Map<DocDto>(docAfter);

        await _notificationService.Notify(docToTransfer);
    }

    public async Task DeleteDocAsync(Guid docId, Guid userId)
    {
        var canUserEdit = await _editorsService.CanUserDo(docId,
            new List<EditorScope>() { EditorScope.CanRead, EditorScope.CanWrite, EditorScope.CanDelete });

        if (!canUserEdit)
        {
            throw new ForbiddenException("User don't have enough rights to do that");
        }

        var saveChangesResult = await _mediator.Send(new GetAndDeleteDocCommand()
        {
            DocId = docId,
            UserId = userId
        });

        if (saveChangesResult != 1)
        {
            throw new GeneralException("Could not remove doc");
        }

        await _notificationService.NotifyDeleted(docId);
    }
}
