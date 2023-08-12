using AutoMapper;
using LiterasData.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS.Queries;

public class GetDocThumbnailsQuery : IRequest<IEnumerable<DocDto>>
{
}
public class GetDocThumbnails : IRequestHandler<GetDocThumbnailsQuery, IEnumerable<DocDto>>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetDocThumbnails(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DocDto>> Handle(GetDocThumbnailsQuery request, CancellationToken cancellationToken)
    {
        return _dbContext.Docs
            .AsNoTracking()
            .Select(doc => new DocDto()
            {
                Id = doc.Id,
                Title = doc.Title
            })
            .AsEnumerable();
    }
}