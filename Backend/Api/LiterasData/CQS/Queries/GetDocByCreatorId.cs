using AutoMapper;
using LiterasData.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS.Queries;

public class GetDocByCreatorIdQuery : IRequest<DocDto>
{
    public Guid CreatorId { get; set; }
}
public class GetDocByCreatorId : IRequestHandler<GetDocByCreatorIdQuery, DocDto>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetDocByCreatorId(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<DocDto> Handle(GetDocByCreatorIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Docs
            .AsNoTracking()
            .FirstOrDefaultAsync(doc => doc.CreatorId == request.CreatorId, cancellationToken: cancellationToken);
        return _mapper.Map<DocDto>(entity);
    }
}