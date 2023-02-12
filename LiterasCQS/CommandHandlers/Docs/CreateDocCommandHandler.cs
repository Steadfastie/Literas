using AutoMapper;
using LiterasCQS.Commands.Docs;
using LiterasData;
using LiterasData.Entities;
using MediatR;

namespace LiterasCQS.Handlers.CommandHandlers.Docs;

public class CreateDocCommandHandler : IRequestHandler<CreateDocCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public CreateDocCommandHandler(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateDocCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Doc>(request.Doc);

        if (entity != null)
        {
            await _context.Docs.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException($"Request contains {request.Doc} user");
        }
    }
}