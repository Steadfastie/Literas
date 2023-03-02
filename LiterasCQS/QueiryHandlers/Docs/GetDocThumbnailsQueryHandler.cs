using AutoMapper;
using LiterasCQS.Queries.Docs;
using LiterasData;
using LiterasDataTransfer.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.QueiryHandlers.Docs;

public class GetDocThumbnailsQueryHandler : IRequestHandler<GetDocThumbnailsQuery, IEnumerable<DocDto>>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetDocThumbnailsQueryHandler(NotesDBContext context, IMapper mapper)
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