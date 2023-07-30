using LiterasDataTransfer.Dto;
using LiterasModels.System;

namespace LiterasDataTransfer.ServiceAbstractions;

public interface IUsersService
{
    Task<UserDto> GetUserByIdAsync(Guid userId);

    Task<CrudResult<UserDto>> CreateUserAsync(UserDto userDto);

    Task<CrudResult<UserDto>> PatchUserAsync(Guid userId, UserDto userDto);

    Task<CrudResult<UserDto>> DeleteUserAsync(Guid userId);
}