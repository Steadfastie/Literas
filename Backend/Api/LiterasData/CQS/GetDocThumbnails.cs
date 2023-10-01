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
        var filteredDocs = await _dbContext.Docs
            .AsNoTracking()
            .Where(doc => doc.Editors.Any(ed => ed.UserId == request.UserId))
            .ToListAsync(cancellationToken);

        foreach (var doc in filteredDocs)
        {
            await _dbContext.Entry(doc)
                .Collection(d => d.Editors)
                .Query()
                .Where(ed => ed.UserId == request.UserId)
                .LoadAsync(cancellationToken);
        }

        return filteredDocs;
    }
}
