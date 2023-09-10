using AutoMapper;
using LiterasCore.Abstractions;
using LiterasData.CQS;
using LiterasData.Entities;
using MediatR;

namespace LiterasCore.Services;

public class EditorsService : IEditorsService
{
    private readonly IIdentityService _identityService;
    private readonly IMediator _mediator;

    public EditorsService(IMediator mediator, IIdentityService identityService)
    {
        _mediator = mediator;
        _identityService = identityService;
    }

    public async Task<bool> CanUserDo(Guid docId, List<EditorScope> scopes)
    {
        var editorScopes = await _mediator.Send(new GetEditorScopes
        {
            DocId = docId, UserId = _identityService.UserId
        });

        return scopes.TrueForAll(scope => editorScopes.Contains(scope));
    }
}
