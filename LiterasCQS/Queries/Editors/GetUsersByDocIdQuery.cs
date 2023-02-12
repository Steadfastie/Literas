using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Queries.Editors;

public class GetUsersByDocIdQuery : IRequest<IEnumerable<UserDto>>
{
    public Guid DocId { get; set; }
}