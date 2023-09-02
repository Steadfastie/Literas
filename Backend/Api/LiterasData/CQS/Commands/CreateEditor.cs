using AutoMapper;
using LiterasData.DTO;
using LiterasData.Entities;
using MediatR;

namespace LiterasData.CQS.Commands;

public class CreateEditorCommand : IRequest<int>
{
    public EditorDto Editor { get; set; }
}

public class CreateEditor : IRequestHandler<CreateEditorCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public CreateEditor(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateEditorCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Editor>(request.Editor);

        if (entity != null)
        {
            await _context.Editors.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        throw new ArgumentException($"Request contains {request.Editor} user");
    }
}
