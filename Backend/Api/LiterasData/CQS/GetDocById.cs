using LiterasData.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS;

public class GetDocById : IRequest<Doc?>
{
    public Guid DocId { get; set; }
    public string UserId { get; set; }
}

public class GetDocByIdHandler : IRequestHandler<GetDocById, Doc?>
{
    private readonly NotesDbContext _dbContext;

    public GetDocByIdHandler(NotesDbContext context)
    {
        _dbContext = context;
    }

    public async Task<Doc?> Handle(GetDocById request, CancellationToken cancellationToken)
    {
        return await _dbContext.Docs
            .AsNoTracking()
            .Where(doc => doc.Editors.SingleOrDefault(ed => 
                              ed.UserId == request.UserId &&
                              ed.DocId == request.DocId) != null)
            .Include(doc => doc.Editors.Single(ed => ed.UserId.Equals(request.UserId, StringComparison.Ordinal)))
            .ThenInclude(editor => editor.Scopes)
            .Include(doc => doc.Editors.Single(ed => ed.UserId.Equals(request.UserId, StringComparison.Ordinal)))
            .ThenInclude(editor => editor.Status)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
