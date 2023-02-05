using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Commands.Contributors;

public class DeleteContributorCommand : IRequest<int>
{
    public ContributorDto Contributor { get; set; }
}