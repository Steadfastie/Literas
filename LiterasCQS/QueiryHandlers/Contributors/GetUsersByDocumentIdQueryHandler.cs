using AutoMapper;
using LiterasCQS.Queries.Contributors;
using LiterasCQS.Queries.Documents;
using LiterasData;
using LiterasDataTransfer.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.QueiryHandlers.Users;

public class GetUsersByDocumentIdQueryHandler : IRequestHandler<GetUsersByDocumentIdQuery, IEnumerable<UserDTO>>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;
    public GetUsersByDocumentIdQueryHandler(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDTO>> Handle(GetUsersByDocumentIdQuery request, CancellationToken cancellationToken)
    {
        var entities = _dbContext.Contributors
            .AsNoTracking()
            .Where(con => con.DocumentId == request.DocumentId)
            .Select(con => con.User);
        return _mapper.Map<IEnumerable<UserDTO>>(entities);
    }
}
