using System.Reflection;
using AutoMapper;
using LiterasCore.Abstractions;
using LiterasCore.System;
using LiterasData.CQS;
using LiterasData.CQS.Commands;
using LiterasData.CQS.Queries;
using LiterasData.DTO;
using MediatR;

namespace LiterasCore.Services;

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

    public async Task<CrudResult<UserDto>> PatchUserAsync(Guid userId, UserDto userDto)
    {
        var sourceDto = await _mediator.Send(new GetUserByIdQuery() { Id = userId });
        if (sourceDto == null)
        {
            throw new ArgumentException("User with provided id does not exist", nameof(userId));
        }

        PropertyInfo[] ignoreFields = new PropertyInfo[]
        {
            userDto.GetType().GetProperty("Id")!,
        };

        var patchList = PatchModelCreator<UserDto>.Generate(sourceDto, userDto, ignoreFields);

        if (Enumerable.Any<PatchModel>(patchList))
        {
            await _mediator.Send(new PatchUserCommand()
            {
                User = userDto,
                PatchList = patchList
            });

            var updated = await _mediator.Send(new GetUserByIdQuery() { Id = userId });

            return new CrudResult<UserDto>(updated);
        }
        else
        {
            return new CrudResult<UserDto>();
        }
    }

    public async Task<CrudResult<UserDto>> DeleteUserAsync(Guid userId)
    {
        var sourceDto = await _mediator.Send(new GetUserByIdQuery() { Id = userId });
        if (sourceDto == null)
        {
            throw new ArgumentNullException(nameof(userId), "User with provided id does not exist");
        }

        await _mediator.Send(new DeleteUserCommand()
        {
            User = sourceDto
        });

        return new CrudResult<UserDto>(sourceDto);
    }
}