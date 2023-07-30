using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Commands.Users;

public class CreateUserCommand : IRequest<int>
{
    public UserDto User { get; set; }
}