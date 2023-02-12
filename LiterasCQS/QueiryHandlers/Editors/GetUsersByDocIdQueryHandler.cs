using AutoMapper;
using LiterasCQS.Queries.Editors;
using LiterasData;
using LiterasDataTransfer.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.QueiryHandlers.Users;

public class GetUsersByDocIdQueryHandler : IRequestHandler<GetUsersByDocIdQuery, IEnumerable<UserDto>>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetUsersByDocIdQueryHandler(NotesDBContext context, IMapper mapper)
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