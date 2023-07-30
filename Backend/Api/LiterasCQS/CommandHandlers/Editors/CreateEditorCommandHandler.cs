using AutoMapper;
using LiterasCQS.Commands.Editors;
using LiterasData;
using LiterasData.Entities;
using MediatR;

namespace LiterasCQS.Handlers.CommandHandlers.Editors;

public class CreateEditorCommandHandler : IRequestHandler<CreateEditorCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public CreateEditorCommandHandler(NotesDBContext context, IMapper mapper)
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
        else
        {
            throw new ArgumentException($"Request contains {request.Editor} user");
        }
    }
}