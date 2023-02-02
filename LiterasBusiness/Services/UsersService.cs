using AutoMapper;
using LiterasCQS.Commands.Users;
using LiterasCQS.Queries.Users;
using LiterasDataTransfer.DTO;
using LiterasDataTransfer.ServiceAbstractions;
using LiterasModels.System;
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

    public async Task<UserDTO> GetUserByIdAsync(Guid userId)
    {
        if (userId != Guid.Empty)
        {
            return await _mediator.Send(new GetUserByIdQuery()
            {
                Id = userId
            });
        }
        else
        {
            throw new ArgumentException("Provided id is empty");
        }
    }

    public async Task<int> CreateUserAsync(UserDTO userDTO)
    {
        if (userDTO != null)
        {
            return await _mediator.Send(new CreateUserCommand()
            {
                User = userDTO
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(UserDTO));
        }
    }

    public async Task<int> PatchUserAsync(UserDTO userDTO, List<PatchModel> patchlist)
    {
        var patchModelsWithId = patchlist
            .Where(l => l.PropertyName
                .Equals("Id", StringComparison.InvariantCultureIgnoreCase));

        if (patchModelsWithId.Any())
        {
            throw new ArgumentException("Id cannot be changed");
        }

        if (userDTO != null)
        {
            return await _mediator.Send(new PatchUserCommand()
            {
                User = userDTO,
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