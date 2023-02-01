using LiterasDataTransfer.DTO;

namespace LiterasDataTransfer.ServiceAbstractions;

public interface IUsersService
{
    Task<int> CreateUserAsync(UserDTO UserDTO);

    Task<int> DeleteUserAsync(UserDTO UserDTO);

    Task<UserDTO> GetUserByIdAsync(Guid UserId);

    Task<int> PatchUserAsync(UserDTO UserDTO, List<PatchModel> patchlist);
}