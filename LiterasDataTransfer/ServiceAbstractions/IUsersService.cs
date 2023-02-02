using LiterasDataTransfer.DTO;
using LiterasModels.System;

namespace LiterasDataTransfer.ServiceAbstractions;

public interface IUsersService
{
    Task<int> CreateUserAsync(UserDTO userDTO);

    Task<int> DeleteUserAsync(UserDTO userDTO);

    Task<UserDTO> GetUserByIdAsync(Guid userId);

    Task<int> PatchUserAsync(UserDTO userDTO, List<PatchModel> patchlist);
}