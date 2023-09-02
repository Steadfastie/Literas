using AutoMapper;
using LiterasData.DTO;
using LiterasData.Entities;
using MediatR;

namespace LiterasData.CQS.Commands;

public class DeleteEditorCommand : IRequest<int>
{
    public EditorDto Editor { get; set; }
}

public class DeleteEditor : IRequestHandler<DeleteEditorCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public DeleteEditor(NotesDBContext context, IMapper mapper)
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

        throw new ArgumentException($"Request contains {request.Editor} user");
    }
}
