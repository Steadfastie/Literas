using LiterasDataTransfer.DTO;
using MediatR;

namespace LiterasCQS.Queries.Contributors;

public class GetContributorByIdQuery : IRequest<ContributorDTO>
{
    public Guid Id { get; set; }
}