using AutoMapper;
using LiterasData.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS;

public class GetAndDeleteDocCommand : IRequest<int>
{
    public Guid DocId { get; set; }
}

public class GetAndDeleteDocHandler : IRequestHandler<GetAndDeleteDocCommand, int>
{
    private readonly NotesDbContext _context;

    public GetAndDeleteDocHandler(NotesDbContext context, IMapper mapper)
    {
        _context = context;
    }

    public async Task<int> Handle(GetAndDeleteDocCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Docs
                         .SingleOrDefaultAsync(
                             doc => doc.Id == request.DocId,
                             cancellationToken) ??
                     throw new NotFoundException("Looks like doc is already gone");

        _context.Docs.Remove(entity);
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
