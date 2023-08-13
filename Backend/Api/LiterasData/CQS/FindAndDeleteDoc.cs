using AutoMapper;
using LiterasData.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS;

public class FindAndDeleteDocCommand : IRequest<int>
{
    public Guid DocId { get; set; }
    public Guid UserId { get; set; }
}

public class FindAndDeleteDocHandler : IRequestHandler<FindAndDeleteDocCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public FindAndDeleteDocHandler(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(FindAndDeleteDocCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Docs
                            .AsNoTracking()
                            .SingleOrDefaultAsync(
                                doc => doc.Id == request.DocId,
                                cancellationToken: cancellationToken) ??
                        throw new NotFoundException("Looks like doc is already gone");

        if (entity.CreatorId != request.UserId)
        {
            throw new ForbiddenException("Only creator can remove doc");
        }

        // Ensure that doc has not been updated
        entity.Version += 1;

        _context.Docs.Remove(entity);
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
