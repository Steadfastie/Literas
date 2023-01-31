using LiterasDataTransfer.DTO;
using MediatR;

namespace LiterasCQS.Commands.Users;

public class DeleteUserCommand : IRequest<int>
{
    public UserDTO User { get; set; }
}