using LiterasDataTransfer.DTO;
using MediatR;

namespace LiterasCQS.Commands.Users;

public class CreateUserCommand : IRequest<int>
{
    public UserDTO User { get; set; }
}