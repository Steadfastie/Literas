using AutoMapper;
using LiterasCQS.Queries.Documents;
using LiterasData;
using LiterasDataTransfer.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.QueiryHandlers.Users;

public class GetDocumentByTitleQueryHandler : IRequestHandler<GetDocumentByTitleQuery, DocumentDto>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetDocumentByTitleQueryHandler(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<DocumentDto> Handle(GetDocumentByTitleQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Documents
            .AsNoTracking()
            .FirstOrDefaultAsync(doc => doc.Title == request.Title, cancellationToken: cancellationToken);
        return _mapper.Map<DocumentDto>(entity);
    }
}