using LiterasData.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS;

public class GetDocThumbnails : IRequest<List<Doc>>
{
    public string UserId { get; set; }
}

public class GetDocThumbnailsHandler : IRequestHandler<GetDocThumbnails, List<Doc>>
{
    private readonly NotesDbContext _dbContext;

    public GetDocThumbnailsHandler(NotesDbContext context)
    {
        _dbContext = context;
    }

    public async Task<List<Doc>> Handle(GetDocThumbnails request, CancellationToken cancellationToken)
    {
        return await _dbContext.Docs
            .AsNoTracking()
            .Where(doc => doc.Editors.SingleOrDefault(ed => ed.UserId.Equals(request.UserId, StringComparison.Ordinal)) != null)
            .Include(doc => doc.Editors.Single(ed => ed.UserId.Equals(request.UserId, StringComparison.Ordinal)))
            .ThenInclude(editor => editor.Scopes)
            .Include(doc => doc.Editors.Single(ed => ed.UserId.Equals(request.UserId, StringComparison.Ordinal)))
            .ThenInclude(editor => editor.Status)
            .ToListAsync(cancellationToken);
    }
}
