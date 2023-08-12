using LiterasCore.System;
using LiterasData.DTO;

namespace LiterasCore.Abstractions;

public interface IEditorsService
{
    Task<CrudResult<EditorDto>> CreateEditorAsync(EditorDto editorDto);

    Task<CrudResult<EditorDto>> DeleteEditorAsync(Guid editorId);

    Task<CrudResult<EditorDto>> GetEditorByIdAsync(Guid editorId);

    Task<CrudResults<IEnumerable<DocDto>>> GetDocsByUserIdAsync(Guid userId);

    Task<CrudResults<IEnumerable<UserDto>>> GetUsersByDocIdAsync(Guid documentId);
}