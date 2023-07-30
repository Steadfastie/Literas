using AutoMapper;
using LiterasCQS.Commands.Docs;
using LiterasData;
using LiterasData.Entities;
using MediatR;

namespace LiterasCQS.Handlers.CommandHandlers.Docs;

public class DeleteDocCommandHandler : IRequestHandler<DeleteDocCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public DeleteDocCommandHandler(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(DeleteDocCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Doc>(request.Doc);

        if (entity != null)
        {
            _context.Docs.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException($"Request contains {request.Doc} user");
        }
    }
}