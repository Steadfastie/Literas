using AutoMapper;
using LiterasData.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS.Queries;

public class GetDocsByUserIdQuery : IRequest<IEnumerable<DocDto>>
{
    public Guid UserId { get; set; }
}
public class GetDocsByUserId : IRequestHandler<GetDocsByUserIdQuery, IEnumerable<DocDto>>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetDocsByUserId(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DocDto>> Handle(GetDocsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var entities = _dbContext.Editors
            .AsNoTracking()
            .Where(con => con.UserId == request.UserId)
            .Select(con => con.Doc);
        return _mapper.Map<IEnumerable<DocDto>>(entities);
    }
}