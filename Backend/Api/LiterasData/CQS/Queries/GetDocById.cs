using AutoMapper;
using LiterasData.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS.Queries;

[RetryPolicy]
public class GetDocByIdQuery : IRequest<DocDto?>
{
    public Guid Id { get; set; }
}

public class GetDocById : IRequestHandler<GetDocByIdQuery, DocDto?>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetDocById(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<DocDto?> Handle(GetDocByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Docs
            .AsNoTracking()
            .FirstOrDefaultAsync(doc => doc.Id == request.Id, cancellationToken: cancellationToken);
        return _mapper.Map<DocDto>(entity);
    }
}