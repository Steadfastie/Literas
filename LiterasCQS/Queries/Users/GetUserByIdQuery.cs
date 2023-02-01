using LiterasDataTransfer.DTO;
using MediatR;

namespace LiterasCQS.Queries.Users;

public class GetUserByIdQuery : IRequest<UserDTO>
{
    public Guid Id { get; set; }
}