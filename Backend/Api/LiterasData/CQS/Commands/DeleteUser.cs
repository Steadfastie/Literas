using AutoMapper;
using LiterasData.DTO;
using LiterasData.Entities;
using MediatR;

namespace LiterasData.CQS.Commands;

public class DeleteUserCommand : IRequest<int>
{
    public UserDto User { get; set; }
}
public class DeleteUser : IRequestHandler<DeleteUserCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public DeleteUser(NotesDBContext context, IMapper mapper)
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