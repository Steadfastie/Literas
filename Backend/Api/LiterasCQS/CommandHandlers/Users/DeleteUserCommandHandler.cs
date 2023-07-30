using AutoMapper;
using LiterasCQS.Commands.Users;
using LiterasData;
using LiterasData.Entities;
using MediatR;

namespace LiterasCQS.Handlers.CommandHandlers.Users;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public DeleteUserCommandHandler(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<User>(request.User);

        if (entity != null)
        {
            _context.Users.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException($"Request contains {request.User} user");
        }
    }
}