using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using LiterasData.DTO;
using LiterasData.Entities;
using LiterasData.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS;

[RetryPolicy]
public class FindAndPatchDocCommand : IRequest<int>
{
    public DocDto Doc { get; set; }
    public Guid UserId { get; set; }
}

public class FindAndPatchDocHandler : IRequestHandler<FindAndPatchDocCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public FindAndPatchDocHandler(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(FindAndPatchDocCommand request, CancellationToken cancellationToken)
    {
        var sourceDoc = await _context.Docs
            .AsNoTracking()
            .SingleOrDefaultAsync(
                doc => doc.Id == request.Doc.Id, 
                cancellationToken: cancellationToken) ?? 
                            throw new NotFoundException("Looks like doc is already here");

        if (sourceDoc.CreatorId != request.UserId)
        {
            throw new ForbiddenException("Only creator can remove doc");
        }

        var (changedDoc, updates) = CalculateChanges(
            sourceDoc, _mapper.Map<Doc>(request.Doc));

        var dbEntityEntry = _context.Entry(changedDoc);

        if (dbEntityEntry.Entity.Version != sourceDoc.Version)
        {
            throw new RaceException("Doc has been updated while patching");
        }

        dbEntityEntry.CurrentValues.SetValues(updates);
        dbEntityEntry.State = EntityState.Modified;

        return await _context.SaveChangesAsync(cancellationToken);
    }

    private (Doc, Dictionary<string, object>) CalculateChanges(Doc souce, Doc changed)
    {
        changed.Version += 1;

        var patchList = PatchModelCreator<Doc>.Generate(souce, changed,
            new[]
            {
                GetPropertyInfo(() => changed.Id),
                GetPropertyInfo(() => changed.CreatorId),
                GetPropertyInfo(() => changed.CreatedAt),
            });

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
        MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
        return (PropertyInfo)memberExpression.Member;
    }
}
