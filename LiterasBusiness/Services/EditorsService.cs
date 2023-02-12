using AutoMapper;
using LiterasCQS.Commands.Editors;
using LiterasCQS.Queries.Editors;
using LiterasDataTransfer.Dto;
using LiterasDataTransfer.ServiceAbstractions;
using MediatR;

namespace LiterasBusiness.Services;

public class EditorsService : IEditorsService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public EditorsService(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<EditorDto> GetEditorByIdAsync(Guid EditorId)
    {
        if (EditorId != Guid.Empty)
        {
            return await _mediator.Send(new GetEditorByIdQuery()
            {
                Id = EditorId
            });
        }
        else
        {
            throw new ArgumentException("Provided id is empty");
        }
    }

    public async Task<IEnumerable<DocDto>> GetDocsByUserIdAsync(Guid userId)
    {
        if (userId != Guid.Empty)
        {
            return await _mediator.Send(new GetDocsByUserIdQuery()
            {
                UserId = userId
            });
        }
        else
        {
            throw new ArgumentException("User id is empty");
        }
    }

    public async Task<IEnumerable<UserDto>> GetUsersByDocIdAsync(Guid documentId)
    {
        if (documentId != Guid.Empty)
        {
            return await _mediator.Send(new GetUsersByDocIdQuery()
            {
                DocId = documentId
            });
        }
        else
        {
            throw new ArgumentException("Doc id is empty");
        }
    }

    public async Task<int> CreateEditorAsync(EditorDto EditorDto)
    {
        if (EditorDto != null)
        {
            return await _mediator.Send(new CreateEditorCommand()
            {
                Editor = EditorDto
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(EditorDto));
        }
    }

    public async Task<int> DeleteEditorAsync(EditorDto EditorDto)
    {
        if (EditorDto != null)
        {
            return await _mediator.Send(new DeleteEditorCommand()
            {
                Editor = EditorDto
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(EditorDto));
        }
    }
}