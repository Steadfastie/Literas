using LiterasData.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS;

[RetryPolicy]
public class GetEditorScopes : IRequest<List<EditorScope>>
{
    public Guid DocId { get; set; }
    public string UserId { get; set; }
}

public class GetEditorScopesHandler : IRequestHandler<GetEditorScopes, List<EditorScope>>
{
    private readonly NotesDbContext _dbContext;

    public GetEditorScopesHandler(NotesDbContext context)
    {
        _dbContext = context;
    }

    public async Task<List<EditorScope>> Handle(GetEditorScopes request, CancellationToken cancellationToken)
    {
        return await _dbContext.Editors
            .AsNoTracking()
            .Where(editor =>
                editor.DocId == request.DocId &&
                editor.UserId.Equals(request.UserId, StringComparison.Ordinal))
            .SelectMany(editor => editor.Scopes)
            .ToListAsync(cancellationToken);
    }
}
