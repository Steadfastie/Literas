using AutoMapper;
using LiterasCQS.Queries.Documents;
using LiterasData;
using LiterasDataTransfer.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.QueiryHandlers.Documents;

public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, DocumentDTO>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetDocumentByIdQueryHandler(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<DocumentDTO> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Documents
            .AsNoTracking()
            .FirstOrDefaultAsync(doc => doc.Id == request.Id, cancellationToken: cancellationToken);
        return _mapper.Map<DocumentDTO>(entity);
    }
}