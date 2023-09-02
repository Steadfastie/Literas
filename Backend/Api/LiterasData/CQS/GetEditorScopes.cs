using LiterasData.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS;

[RetryPolicy]
public class GetEditorScopes : IRequest<List<EditorScope>>
{
    public Guid DocId { get; set; }
    public Guid UserId { get; set; }
}

public class GetEditorScopesHandler : IRequestHandler<GetEditorScopes, List<EditorScope>>
{
    private readonly NotesDBContext _dbContext;

    public GetEditorScopesHandler(NotesDBContext context)
    {
        _dbContext = context;
    }

    public async Task<List<EditorScope>> Handle(GetEditorScopes request, CancellationToken cancellationToken)
    {
        return await _dbContext.Editors
            .AsNoTracking()
            .Where(editor =>
                editor.DocId == request.DocId &&
                editor.UserId == request.UserId)
            .SelectMany(editor => editor.Scopes)
            .ToListAsync(cancellationToken);
    }
}
