using AutoMapper;
using LiterasCQS.Queries.Contributors;
using LiterasData;
using LiterasDataTransfer.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.QueiryHandlers.Users;

public class GetDocumentsByUserIdQueryHandler : IRequestHandler<GetDocumentsByUserIdQuery, IEnumerable<DocumentDto>>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetDocumentsByUserIdQueryHandler(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DocumentDto>> Handle(GetDocumentsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var entities = _dbContext.Contributors
            .AsNoTracking()
            .Where(con => con.UserId == request.UserId)
            .Select(con => con.Document);
        return _mapper.Map<IEnumerable<DocumentDto>>(entities);
    }
}