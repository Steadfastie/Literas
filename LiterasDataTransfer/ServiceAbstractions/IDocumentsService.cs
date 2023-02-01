using LiterasDataTransfer.DTO;

namespace LiterasDataTransfer.ServiceAbstractions;

public interface IDocumentsService
{
    Task<int> CreateDocumentAsync(DocumentDTO documentDTO);

    Task<int> DeleteDocumentAsync(DocumentDTO documentDTO);

    Task<DocumentDTO> GetDocumentByIdAsync(Guid documentId);

    Task<DocumentDTO> GetDocumentByTitleAsync(string title);

    Task<int> PatchDocumentAsync(DocumentDTO documentDTO, List<PatchModel> patchlist);
}