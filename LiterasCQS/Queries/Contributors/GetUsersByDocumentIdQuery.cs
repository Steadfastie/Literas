using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Queries.Contributors;

public class GetUsersByDocumentIdQuery : IRequest<IEnumerable<UserDto>>
{
    public Guid DocumentId { get; set; }
}