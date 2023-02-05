using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Queries.Contributors;

public class GetContributorByIdQuery : IRequest<ContributorDto>
{
    public Guid Id { get; set; }
}