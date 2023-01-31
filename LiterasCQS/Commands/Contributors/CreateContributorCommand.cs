using LiterasDataTransfer.DTO;
using MediatR;

namespace LiterasCQS.Commands.Contributors;

public class CreateContributorCommand : IRequest<int>
{
    public ContributorDTO Contributor { get; set; }
}