using AutoMapper;
using LiterasCQS.Queries.Docs;
using LiterasData;
using LiterasDataTransfer.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.QueiryHandlers.Docs;

public class GetDocByIdQueryHandler : IRequestHandler<GetDocByIdQuery, DocDto>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetDocByIdQueryHandler(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<DocDto> Handle(GetDocByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Docs
            .AsNoTracking()
            .FirstOrDefaultAsync(doc => doc.Id == request.Id, cancellationToken: cancellationToken);
        return _mapper.Map<DocDto>(entity);
    }
}