using AutoMapper;
using LiterasCore.Abstractions;
using LiterasData.CQS;
using LiterasData.DTO;
using LiterasData.Entities;
using LiterasData.Exceptions;
using MediatR;

namespace LiterasCore.Services;

public class DocsService : IDocsService
{
    private readonly IEditorsService _editorsService;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IEventBus _notificationService;

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

    public async Task<List<(DocDto doc, List<EditorScope> scopes, EditorStatus status)>> GetDocThumbnailsAsync(
        CancellationToken cancellationToken = default)
    {
        var docsFound = await _mediator.Send(new GetDocThumbnails { UserId = _identityService.UserId },
            cancellationToken);

        return docsFound.Select(d =>
            {
                var dto = _mapper.Map<DocDto>(d);
                var scopes = d.Editors.Single(ed => ed.UserId == _identityService.UserId).Scopes;
                var status = d.Editors.Single(ed => ed.UserId == _identityService.UserId).Status;
                return (dto, scopes, status);
            })
            .ToList();
    }

    public async Task<(DocDto doc, List<EditorScope> scopes, EditorStatus status)> GetDocByIdAsync(Guid docId,
        CancellationToken cancellationToken = default)
    {
        var docFound = await _mediator.Send(new GetDocById { DocId = docId, UserId = _identityService.UserId },
                           cancellationToken) ??
                       throw new NotFoundException("Could not find such document");

        var dto = _mapper.Map<DocDto>(docFound);
        var scopes = docFound.Editors.Single(ed => ed.UserId == _identityService.UserId).Scopes;
        var status = docFound.Editors.Single(ed => ed.UserId == _identityService.UserId).Status;
        return (dto, scopes, status);
    }

    public async Task<Guid> CreateDocAsync(DocDto newDoc, CancellationToken cancellationToken = default)
    {
        var doc = new Doc(newDoc.Id,
            (newDoc.Title, newDoc.TitleDelta),
            (newDoc.Content, newDoc.ContentDelta));

        var creator = new Editor(_identityService.UserId, newDoc.Id, EditorStatus.Creator,
            new List<EditorScope> { EditorScope.CanRead, EditorScope.CanWrite, EditorScope.CanDelete });

        var saveChangesResult =
            await _mediator.Send(new CreateDocCommand { Doc = doc, Creator = creator }, cancellationToken);

        if (saveChangesResult != 1)
        {
            throw new GeneralException("Doc creation went wrong");
        }

        return doc.Id;
    }

    public async Task PatchDocAsync(DocDto changedDocDto, CancellationToken cancellationToken = default)
    {
        var canUserEdit = await _editorsService.CanUserDo(changedDocDto.Id,
            new List<EditorScope> { EditorScope.CanRead, EditorScope.CanWrite });

        if (!canUserEdit)
        {
            throw new ForbiddenException("User don't have enough rights to do that");
        }

        var changedDoc = new Doc(changedDocDto.Id,
            (changedDocDto.Title, changedDocDto.TitleDelta),
            (changedDocDto.Content, changedDocDto.ContentDelta));

        var docAfter =
            await _mediator.Send(new GetAndPatchDocCommand { Doc = changedDoc, UserId = _identityService.UserId },
                cancellationToken);

        var docToTransfer = _mapper.Map<DocDto>(docAfter);

        await _notificationService.Notify(docToTransfer);
    }

    public async Task DeleteDocAsync(Guid docId, CancellationToken cancellationToken = default)
    {
        var canUserEdit = await _editorsService.CanUserDo(docId,
            new List<EditorScope> { EditorScope.CanRead, EditorScope.CanWrite, EditorScope.CanDelete });

        if (!canUserEdit)
        {
            throw new ForbiddenException("User don't have enough rights to do that");
        }

        var saveChangesResult =
            await _mediator.Send(new GetAndDeleteDocCommand { DocId = docId },
                cancellationToken);

        if (saveChangesResult != 1)
        {
            throw new GeneralException("Could not remove doc");
        }

        await _notificationService.NotifyDeleted(docId);
    }
}
