using LiterasDataTransfer.Dto;

namespace LiterasDataTransfer.ServiceAbstractions;

public interface IContributorsService
{
    Task<int> CreateContributorAsync(ContributorDto ContributorDto);

    Task<int> DeleteContributorAsync(ContributorDto ContributorDto);

    Task<ContributorDto> GetContributorByIdAsync(Guid ContributorId);

    Task<IEnumerable<DocumentDto>> GetDocumentsByUserIdAsync(Guid userId);

    Task<IEnumerable<UserDto>> GetUsersByDocumentIdAsync(Guid documentId);
}