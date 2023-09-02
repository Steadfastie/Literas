using AutoMapper;
using LiterasData.Entities;
using LiterasData.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS;

[RetryPolicy]
public class CreateDocCommand : IRequest<int>
{
    public Doc Doc { get; set; }
    public Editor Creator { get; set; }
}

public class CreateDocHandler : IRequestHandler<CreateDocCommand, int>
{
    private readonly NotesDBContext _context;

    public CreateDocHandler(NotesDBContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateDocCommand request, CancellationToken cancellationToken)
    {
        var presentEntity = await _context.Docs
            .SingleOrDefaultAsync(doc => doc.Id == request.Doc.Id, cancellationToken: cancellationToken);

        if (presentEntity != null)
        {
            throw new RaceException("Looks like doc is already here");
        }

        await _context.Docs.AddAsync(request.Doc, cancellationToken);
        await _context.Editors.AddAsync(request.Creator, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
