using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Commands.Contributors;

public class CreateContributorCommand : IRequest<int>
{
    public ContributorDto Contributor { get; set; }
}