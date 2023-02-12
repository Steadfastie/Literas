using LiterasDataTransfer.Dto;

namespace LiterasDataTransfer.ServiceAbstractions;

public interface IEditorsService
{
    Task<int> CreateEditorAsync(EditorDto EditorDto);

    Task<int> DeleteEditorAsync(EditorDto EditorDto);

    Task<EditorDto> GetEditorByIdAsync(Guid EditorId);

    Task<IEnumerable<DocDto>> GetDocsByUserIdAsync(Guid userId);

    Task<IEnumerable<UserDto>> GetUsersByDocIdAsync(Guid documentId);
}