using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using LiterasData.Entities;
using LiterasData.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS;

[RetryPolicy]
public class GetAndPatchDocCommand : IRequest<Doc>
{
    public Doc Doc { get; set; }
    public string UserId { get; set; }
}

public class GetAndPatchDocHandler : IRequestHandler<GetAndPatchDocCommand, Doc>
{
    private readonly NotesDbContext _context;
    private readonly IMapper _mapper;

    public GetAndPatchDocHandler(NotesDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Doc> Handle(GetAndPatchDocCommand request, CancellationToken cancellationToken)
    {
        var sourceDoc = await _context.Docs.SingleOrDefaultAsync(
                                doc => doc.Id == request.Doc.Id,
                                cancellationToken) ??
                        throw new NotFoundException("Doc missing");

        var editor = await _context.Editors.SingleOrDefaultAsync(
                            ed => ed.DocId == request.Doc.Id &&
                                ed.UserId.Equals(request.UserId, StringComparison.Ordinal),
                            cancellationToken) ??
                        throw new NotFoundException("No such editor for this doc");

        var (changedDoc, updates) = CalculateChanges(
            sourceDoc, request.Doc);

        var docEntry = _context.Entry(changedDoc);
        var editorEntry = _context.Entry(editor);

        docEntry.Entity.UpdateVersion();
        docEntry.CurrentValues.SetValues(updates);
        docEntry.State = EntityState.Modified;

        editorEntry.Entity.Contribute();

        var result = await _context.SaveChangesAsync(cancellationToken);

        if (result != 1)
        {
            throw new NotModifiedException("Doc update went wrong");
        }

        return await _context.Docs
            .AsNoTracking()
            .SingleAsync(doc => doc.Id == request.Doc.Id, cancellationToken);
    }

    private static (Doc, Dictionary<string, object>) CalculateChanges(Doc source, Doc changed)
    {
        var patchList = PatchModelCreator<Doc>.Generate(source, changed,
            new[] { GetPropertyInfo(() => changed.Id), GetPropertyInfo(() => changed.CreatedAt) });

        if (!patchList.Any())
        {
            throw new NotModifiedException("There is nothing to patch");
        }

        var nameValuePropertiesPairs = patchList
            .ToDictionary(
                patchModel => patchModel.PropertyName,
                patchModel => patchModel.PropertyValue);

        return (changed, nameValuePropertiesPairs);
    }

    private static PropertyInfo GetPropertyInfo<TSource>(Expression<Func<TSource>> propertyExpression)
    {
        var memberExpression = (MemberExpression)propertyExpression.Body;
        return (PropertyInfo)memberExpression.Member;
    }
}
