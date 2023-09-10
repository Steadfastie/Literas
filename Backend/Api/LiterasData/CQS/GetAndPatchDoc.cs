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
        var sourceDoc = await _context.Docs
                            .SingleOrDefaultAsync(
                                doc => doc.Id == request.Doc.Id,
                                cancellationToken) ??
                        throw new NotFoundException("Looks like doc is already here");

        var (changedDoc, updates) = CalculateChanges(
            sourceDoc, request.Doc);

        var dbEntityEntry = _context.Entry(changedDoc);

        dbEntityEntry.Entity.UpdateVersion();
        dbEntityEntry.CurrentValues.SetValues(updates);
        dbEntityEntry.State = EntityState.Modified;

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
