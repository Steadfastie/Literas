using AutoMapper;
using LiterasCQS.Queries.Contributors;
using LiterasData;
using LiterasDataTransfer.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.QueiryHandlers.Users;

public class GetContributorByTitleQueryHandler : IRequestHandler<GetContributorByIdQuery, ContributorDto>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetContributorByTitleQueryHandler(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<ContributorDto> Handle(GetContributorByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Contributors
            .AsNoTracking()
            .FirstOrDefaultAsync(con => con.Id == request.Id, cancellationToken: cancellationToken);
        return _mapper.Map<ContributorDto>(entity);
    }
}