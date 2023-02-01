using AutoMapper;
using LiterasCQS.Commands.Users;
using LiterasCQS.Queries.Users;
using LiterasDataTransfer;
using LiterasDataTransfer.DTO;
using LiterasDataTransfer.ServiceAbstractions;
using MediatR;

namespace LiterasBusiness.Services;

public class UsersService : IUsersService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UsersService(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<UserDTO> GetUserByIdAsync(Guid UserId)
    {
        if (UserId != Guid.Empty)
        {
            return await _mediator.Send(new GetUserByIdQuery()
            {
                Id = UserId
            });
        }
        else
        {
            throw new ArgumentException("Provided id is empty");
        }
    }

    public async Task<int> CreateUserAsync(UserDTO UserDTO)
    {
        if (UserDTO != null)
        {
            return await _mediator.Send(new CreateUserCommand()
            {
                User = UserDTO
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(UserDTO));
        }
    }

    public async Task<int> PatchUserAsync(UserDTO UserDTO, List<PatchModel> patchlist)
    {
        var patchModelsWithId = patchlist
            .Where(l => l.PropertyName
                .Equals("Id", StringComparison.InvariantCultureIgnoreCase));

        if (patchModelsWithId.Any())
        {
            throw new ArgumentException("Id cannot be changed");
        }

        if (UserDTO != null)
        {
            return await _mediator.Send(new PatchUserCommand()
            {
                User = UserDTO,
                PatchList = patchlist
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(UserDTO));
        }
    }

    public async Task<int> DeleteUserAsync(UserDTO UserDTO)
    {
        if (UserDTO != null)
        {
            return await _mediator.Send(new DeleteUserCommand()
            {
                User = UserDTO
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(UserDTO));
        }
    }
}