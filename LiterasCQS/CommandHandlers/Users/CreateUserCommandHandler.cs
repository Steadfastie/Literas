using AutoMapper;
using LiterasCQS.Commands.Users;
using LiterasData;
using LiterasData.Entities;
using MediatR;

namespace LiterasCQS.Handlers.CommandHandlers.Users;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<User>(request.User);

        if (entity != null)
        {
            await _context.Users.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException($"Request contains {request.User} user");
        }
    }
}