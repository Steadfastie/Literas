using AutoMapper;
using LiterasData.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS.Queries;

public class GetUsersByDocIdQuery : IRequest<IEnumerable<UserDto>>
{
    public Guid DocId { get; set; }
}
public class GetUsersByDocId : IRequestHandler<GetUsersByDocIdQuery, IEnumerable<UserDto>>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetUsersByDocId(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersByDocIdQuery request, CancellationToken cancellationToken)
    {
        var entities = _dbContext.Editors
            .AsNoTracking()
            .Where(con => con.DocId == request.DocId)
            .Select(con => con.User);
        return _mapper.Map<IEnumerable<UserDto>>(entities);
    }
}