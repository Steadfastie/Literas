using AutoMapper;
using LiterasCQS.Commands.Users;
using LiterasCQS.Queries.Users;
using LiterasDataTransfer.Dto;
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

    public async Task<UserDto> GetUserByIdAsync(Guid userId)
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

    public async Task<CrudResult<UserDto>> CreateUserAsync(UserDto userDto)
    {
        if (userDto == null)
        {
            throw new ArgumentNullException(nameof(userDto));
        }

        Guid userId;
        if (userDto.Id == Guid.Empty)
        {
            userId = Guid.NewGuid();
            userDto.Id = userId;
        }
        else
        {
            userId = userDto.Id;
        }

        int commandResult = await _mediator.Send(new CreateUserCommand()
        {
            User = userDto
        });

        if (commandResult == 1)
        {
            var createdUser = await _mediator.Send(new GetUserByIdQuery()
            {
                Id = userId
            });

            return new CrudResult<UserDto>(createdUser);
        }
        else
        {
            return new CrudResult<UserDto>();
        }
    }

    public async Task<int> PatchUserAsync(UserDto userDto, List<PatchModel> patchlist)
    {
        var patchModelsWithId = patchlist
            .Where(l => l.PropertyName
                .Equals("Id", StringComparison.InvariantCultureIgnoreCase));

        if (patchModelsWithId.Any())
        {
            throw new ArgumentException("Id cannot be changed");
        }

        if (userDto != null)
        {
            return await _mediator.Send(new PatchUserCommand()
            {
                User = userDto,
                PatchList = patchlist
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(UserDto));
        }
    }

    public async Task<int> DeleteUserAsync(UserDto UserDto)
    {
        if (UserDto != null)
        {
            return await _mediator.Send(new DeleteUserCommand()
            {
                User = UserDto
            });
        }
        else
        {
            throw new ArgumentNullException(nameof(UserDto));
        }
    }
}