using LiterasDataTransfer.Dto;
using LiterasModels.System;

namespace LiterasDataTransfer.ServiceAbstractions;

public interface IUsersService
{
    Task<UserDto> GetUserByIdAsync(Guid userId);
    Task<CrudResult<UserDto>> CreateUserAsync(UserDto userDto);

    Task<int> DeleteUserAsync(UserDto userDto);

    Task<int> PatchUserAsync(UserDto userDto, List<PatchModel> patchlist);
}