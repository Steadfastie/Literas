using LiterasCore.System;
using LiterasData.DTO;

namespace LiterasCore.Abstractions;

public interface IDocsService
{
    Task<CrudResult<DocDto>> GetDocByIdAsync(Guid docId);

    Task<CrudResults<IEnumerable<DocDto>>> GetDocThumbnailsAsync();

    Task<CrudResult<DocDto>> GetDocByCreatorIdAsync(Guid creatorId);

    Task<CrudResult<DocDto>> GetDocByTitleAsync(string title);

    Task<Guid> CreateDocAsync(DocDto newDoc);

    Task PatchDocAsync(DocDto docDto);

    Task DeleteDocAsync(Guid docId, Guid userId);
}
