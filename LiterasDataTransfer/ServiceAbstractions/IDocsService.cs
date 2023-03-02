using LiterasDataTransfer.Dto;
using LiterasModels.System;

namespace LiterasDataTransfer.ServiceAbstractions;

public interface IDocsService
{
    Task<CrudResult<DocDto>> GetDocByIdAsync(Guid docId);

    Task<CrudResults<IEnumerable<DocDto>>> GetDocThumbnailsAsync();

    Task<CrudResult<DocDto>> GetDocByCreatorIdAsync(Guid creatorId);

    Task<CrudResult<DocDto>> GetDocByTitleAsync(string title);

    Task<CrudResult<DocDto>> CreateDocAsync(DocDto docDto);

    Task<CrudResult<DocDto>> PatchDocAsync(Guid docId, DocDto docDto);

    Task<CrudResult<DocDto>> DeleteDocAsync(Guid docId);
}