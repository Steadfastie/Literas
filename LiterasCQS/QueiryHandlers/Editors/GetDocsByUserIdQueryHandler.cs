using AutoMapper;
using LiterasCQS.Queries.Editors;
using LiterasData;
using LiterasDataTransfer.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.QueiryHandlers.Users;

public class GetDocsByUserIdQueryHandler : IRequestHandler<GetDocsByUserIdQuery, IEnumerable<DocDto>>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetDocsByUserIdQueryHandler(NotesDBContext context, IMapper mapper)
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