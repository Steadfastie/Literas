using LiterasDataTransfer.DTO;
using MediatR;

namespace LiterasCQS.Commands.Contributors;

public class DeleteContributorCommand : IRequest<int>
{
    public ContributorDTO Contributor { get; set; }
}