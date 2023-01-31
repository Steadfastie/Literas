using AutoMapper;
using LiterasCQS.Commands.Documents;
using LiterasData;
using LiterasData.Entities;
using MediatR;

namespace LiterasCQS.Handlers.CommandHandlers.Documents;

public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public DeleteDocumentCommandHandler(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Document>(request.Document);

        if (entity != null)
        {
            _context.Documents.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException($"Request contains {request.Document} user");
        }
    }
}