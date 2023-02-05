using AutoMapper;
using LiterasCQS.Queries.Contributors;
using LiterasData;
using LiterasDataTransfer.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.QueiryHandlers.Users;

public class GetUsersByDocumentIdQueryHandler : IRequestHandler<GetUsersByDocumentIdQuery, IEnumerable<UserDto>>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetUsersByDocumentIdQueryHandler(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersByDocumentIdQuery request, CancellationToken cancellationToken)
    {
        var entities = _dbContext.Contributors
            .AsNoTracking()
            .Where(con => con.DocumentId == request.DocumentId)
            .Select(con => con.User);
        return _mapper.Map<IEnumerable<UserDto>>(entities);
    }
}