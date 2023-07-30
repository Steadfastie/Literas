using AutoMapper;
using LiterasCQS.Commands.Editors;
using LiterasData;
using LiterasData.Entities;
using MediatR;

namespace LiterasCQS.Handlers.CommandHandlers.Editors;

public class DeleteEditorCommandHandler : IRequestHandler<DeleteEditorCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public DeleteEditorCommandHandler(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(DeleteEditorCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Editor>(request.Editor);

        if (entity != null)
        {
            _context.Editors.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException($"Request contains {request.Editor} user");
        }
    }
}