using AutoMapper;
using LiterasCore.Abstractions;
using LiterasCore.System;
using LiterasData.CQS;
using LiterasData.CQS.Commands;
using LiterasData.CQS.Queries;
using LiterasData.DTO;
using LiterasData.Entities;
using MediatR;

namespace LiterasCore.Services;

public class EditorsService : IEditorsService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IIdentityService _identityService;

    public EditorsService(IMapper mapper, IMediator mediator, IIdentityService identityService)
    {
        _mapper = mapper;
        _mediator = mediator;
        _identityService = identityService;
    }

    public async Task<bool> CanUserDo(Guid docId, List<EditorScope> scopes)
    {
        var editorScopes = await _mediator.Send(new GetEditorScopes()
        {
            DocId = docId,
            UserId = _identityService.UserId
        });

        return scopes.TrueForAll(scope => editorScopes.Contains(scope));
    }
}
