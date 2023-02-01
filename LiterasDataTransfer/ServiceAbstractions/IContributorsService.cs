using LiterasDataTransfer.DTO;

namespace LiterasDataTransfer.ServiceAbstractions;

public interface IContributorsService
{
    Task<int> CreateContributorAsync(ContributorDTO ContributorDTO);

    Task<int> DeleteContributorAsync(ContributorDTO ContributorDTO);

    Task<ContributorDTO> GetContributorByIdAsync(Guid ContributorId);

    Task<IEnumerable<DocumentDTO>> GetDocumentsByUserIdAsync(Guid userId);

    Task<IEnumerable<UserDTO>> GetUsersByDocumentIdAsync(Guid documentId);
}