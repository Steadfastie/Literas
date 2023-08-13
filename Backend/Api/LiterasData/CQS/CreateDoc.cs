using AutoMapper;
using LiterasData.DTO;
using LiterasData.Entities;
using LiterasData.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS;

[RetryPolicy]
public class CreateDocCommand : IRequest<int>
{
    public DocDto Doc { get; set; }
}

public class CreateDocHandler : IRequestHandler<CreateDocCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public CreateDocHandler(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateDocCommand request, CancellationToken cancellationToken)
    {
        var presentEntity = await _context.Docs
            .AsNoTracking()
            .SingleOrDefaultAsync(doc => doc.Id == request.Doc.Id ||
                (doc.CreatorId == request.Doc.CreatorId &&
                doc.CreatedAt == request.Doc.CreatedAt), cancellationToken: cancellationToken);

        if (presentEntity != null)
        {
            throw new RaceException("Looks like doc is already here");
        }

        var entity = _mapper.Map<Doc>(request.Doc);

        await _context.Docs.AddAsync(entity, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
