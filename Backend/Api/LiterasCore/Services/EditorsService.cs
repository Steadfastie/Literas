using AutoMapper;
using LiterasCore.Abstractions;
using LiterasData.CQS;
using LiterasData.Entities;
using MediatR;

namespace LiterasCore.Services;

public class EditorsService : IEditorsService
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public EditorsService(IMapper mapper, IMediator mediator, IIdentityService identityService)
    {
        _mapper = mapper;
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
