using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Commands.Users;

public class DeleteUserCommand : IRequest<int>
{
    public UserDto User { get; set; }
}