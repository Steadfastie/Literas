using AutoMapper;
using LiterasData.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS.Queries;

public class GetDocByTitleQuery : IRequest<DocDto>
{
    public string Title { get; set; }
}
public class GetDocByTitle : IRequestHandler<GetDocByTitleQuery, DocDto>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetDocByTitle(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<DocDto> Handle(GetDocByTitleQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Docs
            .AsNoTracking()
            .FirstOrDefaultAsync(doc => doc.Title == request.Title, cancellationToken: cancellationToken);
        return _mapper.Map<DocDto>(entity);
    }
}