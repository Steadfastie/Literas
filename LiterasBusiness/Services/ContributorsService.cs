using AutoMapper;
using LiterasCQS.Commands.Contributors;
using LiterasCQS.Queries.Contributors;
using LiterasDataTransfer.DTO;
using LiterasDataTransfer.ServiceAbstractions;
using MediatR;

namespace LiterasBusiness.Services;

public class ContributorsService : IContributorsService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ContributorsService(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<ContributorDTO> GetContributorByIdAsync(Guid ContributorId)
    {
        if (ContributorId != Guid.Empty)
        {
            return await _mediator.Send(new GetContributorByIdQuery()
            {
                Id = ContributorId
            });
        }
        else
        {
            throw new ArgumentException("Provided id is empty");
        }
    }

    public async Task<IEnumerable<DocumentDTO>> GetDocumentsByUserIdAsync(Guid userId)
    {
        if (userId != Guid.Empty)
        {
            return await _mediator.Send(new GetDocumentsByUserIdQuery()
            {
                UserId = userId
            });
        }
        else
        {
            throw new ArgumentException("User id is empty");
        }
    }

    public async Task<IEnumerable<UserDTO>> GetUsersByDocumentIdAsync(Guid documentId)
    {
        if (documentId != Guid.Empty)
        {
            return await _mediator.Send(new GetUsersByDocumentIdQuery()
            {
                DocumentId = documentId
            });
        }
        else
        {
            throw new ArgumentException("Document id is empty");
        }
    }

    public async Task<int> CreateContributorAsync(ContributorDTO ContributorDTO)
    {
        if (ContributorDTO != null)
        {
            return await _mediator.Send(new CreateContributorCommand()
            {
                Contributor = ContributorDTO
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(ContributorDTO));
        }
    }

    public async Task<int> DeleteContributorAsync(ContributorDTO ContributorDTO)
    {
        if (ContributorDTO != null)
        {
            return await _mediator.Send(new DeleteContributorCommand()
            {
                Contributor = ContributorDTO
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(ContributorDTO));
        }
    }
}