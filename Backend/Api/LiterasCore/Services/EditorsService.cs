using AutoMapper;
using LiterasCore.Abstractions;
using LiterasCore.System;
using LiterasData.CQS.Commands;
using LiterasData.CQS.Queries;
using LiterasData.DTO;
using MediatR;

namespace LiterasCore.Services;

public class EditorsService : IEditorsService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public EditorsService(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<CrudResult<EditorDto>> GetEditorByIdAsync(Guid editorId)
    {
        if (editorId == Guid.Empty)
        {
            throw new ArgumentException(
                $"Provided ID (..{editorId.ToString()[^5..]}) is empty");
        }

        var editorDto = await _mediator.Send(new GetEditorByIdQuery()
        {
            Id = editorId
        });

        return new CrudResult<EditorDto>(editorDto);
    }

    public async Task<CrudResults<IEnumerable<DocDto>>> GetDocsByUserIdAsync(Guid userId)
    {
        if (userId != Guid.Empty)
        {
            throw new ArgumentException($"User ID (..{userId.ToString()[^5..]}) is empty");
        }

        var docsDtos = await _mediator.Send(new GetDocsByUserIdQuery()
        {
            UserId = userId
        });

        return docsDtos.Any()
            ? new CrudResults<IEnumerable<DocDto>>(docsDtos)
            : new CrudResults<IEnumerable<DocDto>>();
    }

    public async Task<CrudResults<IEnumerable<UserDto>>> GetUsersByDocIdAsync(Guid docId)
    {
        if (docId != Guid.Empty)
        {
            throw new ArgumentException(
                $"Doc ID (..{docId.ToString()[^5..]}) is empty");
        }

        var usersDtos = await _mediator.Send(new GetUsersByDocIdQuery()
        {
            DocId = docId
        });

        return usersDtos.Any()
            ? new CrudResults<IEnumerable<UserDto>>(usersDtos)
            : new CrudResults<IEnumerable<UserDto>>();
    }

    public async Task<CrudResult<EditorDto>> CreateEditorAsync(EditorDto editorDto)
    {
        var userDto = await _mediator.Send(new GetUserByIdQuery() { Id = editorDto.UserId });
        var docDto = await _mediator.Send(new GetDocByIdQuery() { Id = editorDto.DocId });

        if (userDto == null)
        {
            throw new ArgumentException(
            $"User's ID (..{editorDto.UserId.ToString()[^5..]}) is invalid");
        }

        if (docDto == null)
        {
            throw new ArgumentException(
            $"Doc's ID (..{editorDto.DocId.ToString()[^5..]}) is invalid");
        }

        if (editorDto.Id == Guid.Empty) editorDto.Id = Guid.NewGuid();

        var saveChangesResult = await _mediator.Send(new CreateEditorCommand()
        {
            Editor = editorDto
        });

        return saveChangesResult == 1
            ? new CrudResult<EditorDto>(editorDto)
            : new CrudResult<EditorDto>();
    }

    public async Task<CrudResult<EditorDto>> DeleteEditorAsync(Guid editorId)
    {
        var sourceDto = await _mediator.Send(new GetEditorByIdQuery() { Id = editorId });
        if (sourceDto == null)
        {
            throw new ArgumentException(
                $"Editor's ID {editorId.ToString()[^5..]}) is invalid");
        }

        var saveChangesResult = await _mediator.Send(
            new DeleteEditorCommand() { Editor = sourceDto });

        return saveChangesResult == 1
            ? new CrudResult<EditorDto>(sourceDto)
            : new CrudResult<EditorDto>();
    }
}