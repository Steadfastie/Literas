using AutoMapper;
using LiterasData.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS.Queries;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public Guid Id { get; set; }
}
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