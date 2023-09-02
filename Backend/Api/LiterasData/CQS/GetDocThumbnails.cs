using LiterasData.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS;

public class GetDocThumbnails : IRequest<List<Doc>>
{
    public Guid UserId { get; set; }
}

public class GetDocThumbnailsHandler : IRequestHandler<GetDocThumbnails, List<Doc>>
{
    private readonly NotesDBContext _dbContext;

    public GetDocThumbnailsHandler(NotesDBContext context)
    {
        _dbContext = context;
    }

    public async Task<List<Doc>> Handle(GetDocThumbnails request, CancellationToken cancellationToken)
    {
        return await _dbContext.Docs
            .AsNoTracking()
            .Where(doc => doc.Editors.SingleOrDefault(ed => ed.UserId == request.UserId) != null)
            .Include(doc => doc.Editors.Single(ed => ed.UserId == request.UserId))
            .ThenInclude(editor => editor.Scopes)
            .Include(doc => doc.Editors.Single(ed => ed.UserId == request.UserId))
            .ThenInclude(editor => editor.Status)
            .ToListAsync(cancellationToken);
    }
}
