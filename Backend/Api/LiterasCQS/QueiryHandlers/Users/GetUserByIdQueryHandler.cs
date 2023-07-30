using AutoMapper;
using LiterasCQS.Queries.Users;
using LiterasData;
using LiterasDataTransfer.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.QueiryHandlers.Users;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetUserByIdQueryHandler(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == request.Id, cancellationToken: cancellationToken);
        return _mapper.Map<UserDto>(entity);
    }
}