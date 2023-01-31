using AutoMapper;
using LiterasCQS.Queries.Documents;
using LiterasData;
using LiterasDataTransfer.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.QueiryHandlers.Users;

public class GetDocumentsByUserIdQueryHandler : IRequestHandler<GetDocumentsByUserIdQuery, IEnumerable<DocumentDTO>>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;
    public GetDocumentsByUserIdQueryHandler(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DocumentDTO>> Handle(GetDocumentsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var entities = _dbContext.Contributors
            .AsNoTracking()
            .Where(con => con.UserId == request.UserId)
            .Select(con => con.Document);
        return _mapper.Map<IEnumerable<DocumentDTO>>(entities);
    }
}
