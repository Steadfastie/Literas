using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Queries.Users;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public Guid Id { get; set; }
}